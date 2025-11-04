using UnityEngine;
using KinematicCharacterController;
using System.IO;
using Unity.VisualScripting;

public enum CrouchInput
{
    None,
    Toggle
}

public enum Stance
{
    Stand,
    Crouch,
    Slide
}

public struct CharacterState
{
    public bool Grounded;
    public Stance stance;
    public Vector3 Velocity;
}

public struct CharacterInput
{
    public Quaternion Rotation;
    public Vector2 Move;
    public bool Jump;
    public bool JumpSustain;
    public CrouchInput Crouch;
}

public class GoblinCharacter : MonoBehaviour, ICharacterController
{
    [SerializeField] bool canMove = true;
    [SerializeField] bool canLook = true;

    [SerializeField] private KinematicCharacterMotor motor;
    [SerializeField] private Transform root;
    [SerializeField] private Transform cameraTarget;
    public Transform CameraTarget => cameraTarget;
    [Space]
    [SerializeField] float walkSpeed = 20f;
    [SerializeField] float crouchSpeed = 7f;
    [SerializeField] float walkResponse = 25f;
    [SerializeField] float crouchResponse = 20f;
    [Space]
    [SerializeField] float airSpeed = 15f;
    [SerializeField] float airAcceleration = 70f;
    [SerializeField] float jumpSpeed = 20f;
    [Range(0, 1), SerializeField] float jumpSustainGravity = 0.4f;
    [SerializeField] float gravity = -90f;
    [Space]
    [Tooltip("Speed gained at the beginning of a slide"), SerializeField] float slideStartSpeed = 25f;
    [Tooltip("Minimum speed for player to be considered sliding"), SerializeField] float slideEndSpeed = 15f;
    [Tooltip("Rate at which the player loses slide speed"), SerializeField] float slideFriction = 0.8f;
    [Tooltip("Rate at which sliding can be steered"), SerializeField] float slideSteerAcceleration = 5f;
    [SerializeField] float slideGravity = -90f;
    [Space]
    [SerializeField] float standHeight = 2f;
    [SerializeField] float crouchHeight = 1f;
    [SerializeField] float crouchHeightResponse = 15f;
    [Space]
    [Range(0, 1.5f), SerializeField] float standCameraTargetHeight = 0.9f;
    [Range(0, 1), SerializeField] float crouchCameraTargetHeight = 0.7f;

    private CharacterState _state;
    private CharacterState _lastState;
    private CharacterState _tempState;

    private Quaternion _requestedRotation;
    private Vector3 _requestedMovement;
    public bool RequestingMovement => _requestedMovement != Vector3.zero;
    private bool _requestedJump;
    private bool _requestedJumpSustain;
    private bool _requestedCrouch;

    private Collider[] _uncrouchOverlapColliders;

    public void Initialize()
    {
        _state.stance = Stance.Stand;
        _lastState = _state;

        _uncrouchOverlapColliders = new Collider[8];

        motor.CharacterController = this;
    }

    public void UpdateInput(CharacterInput input)
    {
        if (canLook)
        {
            _requestedRotation = input.Rotation;
        }
        else
        {
            _requestedRotation = Quaternion.identity;
        }


        if (canMove)
        {
            _requestedMovement = new Vector3(input.Move.x, 0f, input.Move.y);
            _requestedMovement = Vector3.ClampMagnitude(_requestedMovement, 1f);
            _requestedMovement = input.Rotation * _requestedMovement;

            _requestedJump = _requestedJump || input.Jump;
            _requestedJumpSustain = input.JumpSustain;

            _requestedCrouch = input.Crouch switch
            {
                CrouchInput.Toggle => !_requestedCrouch,
                CrouchInput.None => _requestedCrouch,
                _ => _requestedCrouch
            };
        }
        else
        {
            _requestedMovement = Vector3.zero;
            _requestedJump = false;
            _requestedJumpSustain = false;
            _requestedCrouch = false;
        }
    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        var forward = Vector3.ProjectOnPlane
        (
            _requestedRotation * Vector3.forward,
            motor.CharacterUp
        );
        currentRotation = Quaternion.LookRotation(forward, motor.CharacterUp);
    }

    public void UpdateBody(float deltaTime)
    {
        var currentHeight = motor.Capsule.height;
        var normalizedHeight = currentHeight / standHeight;
        var cameraTargetHeight = currentHeight *
        (
            _state.stance is Stance.Stand
                ? standCameraTargetHeight
                : crouchCameraTargetHeight
        );
        //var rootTargetScale = new Vector3(1f, normalizedHeight, 1f);

        cameraTarget.localPosition = Vector3.Lerp
        (
            a: cameraTarget.localPosition,
            b: new Vector3(0f, cameraTargetHeight, 0f),
            t: 1f - Mathf.Exp(-crouchHeightResponse * deltaTime)
        );
        // root.localScale = Vector3.Lerp
        // (
        //     a: root.localScale,
        //     b: rootTargetScale,
        //     t: 1f - Mathf.Exp(-crouchHeightResponse * deltaTime)
        // );
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        // If on ground...
        if (motor.GroundingStatus.IsStableOnGround)
        {
            var groundedMovement = motor.GetDirectionTangentToSurface
            (
                direction: _requestedMovement,
                surfaceNormal: motor.GroundingStatus.GroundNormal
            ) * _requestedMovement.magnitude;

            // Start sliding.
            {
                var moving = groundedMovement.sqrMagnitude > 0f;
                var crouching = _state.stance is Stance.Crouch;
                var wasStanding = _lastState.stance is Stance.Stand;
                var wasInAir = !_lastState.Grounded;
                if (moving && crouching && (wasStanding || wasInAir))
                {
                    _state.stance = Stance.Slide;

                    // When landing on stable ground the character motor projects the velocity onto a flat ground plane.
                    // See: KinematicCharacterMotor.HandleVelocityProjection()
                    // This is normally good, because under normal circumstances the player shouldn't slide when landing on the ground.
                    // In this case, we want the player to slide.
                    // Reproject the last frames (falling) velocity onto the ground normal to slide.
                    if (wasInAir)
                    {
                        currentVelocity = Vector3.ProjectOnPlane
                        (
                            vector: _lastState.Velocity,
                            planeNormal: motor.GroundingStatus.GroundNormal
                        );
                    }

                    var slideSpeed = Mathf.Max(slideStartSpeed, currentVelocity.magnitude);
                    currentVelocity = motor.GetDirectionTangentToSurface
                    (
                        direction: currentVelocity,
                        surfaceNormal: motor.GroundingStatus.GroundNormal
                    ) * slideSpeed;
                }
            }

            // Move on ground.
            if (_state.stance is Stance.Stand or Stance.Crouch)
            {
                // Set speed and responsiveness based on stance.
                var speed = _state.stance is Stance.Stand
                    ? walkSpeed
                    : crouchSpeed;

                var response = _state.stance is Stance.Stand
                    ? walkResponse
                    : crouchResponse;

                var targetVelocity = groundedMovement * speed;
                currentVelocity = Vector3.Lerp
                (
                    a: currentVelocity,
                    b: targetVelocity,
                    t: 1f - Mathf.Exp(-response * deltaTime)
                );
            }
            // Slide on ground.
            else
            {
                // Friction for sliding.
                currentVelocity -= currentVelocity * (slideFriction * Time.deltaTime);

                // Slope.
                {
                    var force = Vector3.ProjectOnPlane
                    (
                        vector: -motor.CharacterUp,
                        planeNormal: motor.GroundingStatus.GroundNormal
                    ) * slideGravity;

                    currentVelocity -= force * deltaTime;
                }

                // Steer.
                {
                    var currentSpeed = currentVelocity.magnitude;
                    var targetVelocity = groundedMovement * currentSpeed;
                    var steerForce = (targetVelocity - currentVelocity) * slideSteerAcceleration * deltaTime;
                    // Add steer force, but clamp speed so the slide doesn't accelerate due to direct movement input.
                    currentVelocity += steerForce;
                    currentVelocity = Vector3.ClampMagnitude(currentVelocity, currentSpeed);
                }

                // Stop.
                if (currentVelocity.magnitude < slideEndSpeed)
                    _state.stance = Stance.Crouch;
            }
        }
        // If in the air...
        else
        {
            // In air movement.
            if (_requestedMovement.sqrMagnitude > 0f)
            {
                var planarMovement = Vector3.ProjectOnPlane
                (
                    vector: _requestedMovement,
                    planeNormal: motor.CharacterUp
                ) * _requestedMovement.magnitude;

                // Current velocity on in-air movement plane.
                var currentPlanarVelocity = Vector3.ProjectOnPlane
                (
                    vector: currentVelocity,
                    planeNormal: motor.CharacterUp
                );

                // Calculate movement force.
                // Will be changed depending on current velocity.
                var movementForce = planarMovement * airAcceleration * deltaTime;

                // If moving slower than the max air speed, treat movementForce as a simple steering force.
                if (currentPlanarVelocity.magnitude < airSpeed)
                {
                    // Add it to the current planar velocity for a target velocity.
                    var targetPlanarVelocity = currentPlanarVelocity + movementForce;

                    // Limit target velocity to air speed.
                    targetPlanarVelocity = Vector3.ClampMagnitude(targetPlanarVelocity, airSpeed);

                    // Steer towards target velocity.
                    movementForce = targetPlanarVelocity - currentPlanarVelocity;
                }
                // Otherwise, nerf the movement force when it is in the direction of the current planar velocity
                // to prevent accelerating futher beyond the max air speed.
                else if (Vector3.Dot(currentPlanarVelocity, movementForce) > 0f)
                {
                    // Project movement force onto the plan whose normal is the current planar velocity.
                    var constrainedMovementForce = Vector3.ProjectOnPlane
                    (
                        vector: movementForce,
                        planeNormal: currentPlanarVelocity.normalized
                    );

                    movementForce = constrainedMovementForce;
                }

                // Prevent air-climbing steep slopes.
                if (motor.GroundingStatus.FoundAnyGround)
                {
                    // If moving in the same direction as the resultant velocity...
                    if (Vector3.Dot(movementForce, currentVelocity + movementForce) > 0f)
                    {
                        // Calculate obstruction normal.
                        var obstructionNormal = Vector3.Cross
                        (
                            motor.CharacterUp,
                            Vector3.Cross
                            (
                                motor.CharacterUp,
                                motor.GroundingStatus.GroundNormal
                            )
                        ).normalized;

                        // Project movement force onto obstruction plane.
                        movementForce = Vector3.ProjectOnPlane(movementForce, obstructionNormal);
                    }
                }

                // Steer towards current velocity.
                currentVelocity += movementForce;
            }

            // Gravity
            var effectiveGravity = gravity;
            var verticalSpeed = Vector3.Dot(currentVelocity, motor.CharacterUp);
            if (_requestedJumpSustain && verticalSpeed > 0f)
                effectiveGravity *= jumpSustainGravity;

            currentVelocity += motor.CharacterUp * effectiveGravity * deltaTime;
        }

        if (_requestedJump)
        {
            var grounded = motor.GroundingStatus.IsStableOnGround;

            if (grounded)
            {
                _requestedJump = false;
                _requestedCrouch = false;

                motor.ForceUnground(time: 0);

                // Set minimum vertical speed to the jump speed.
                var currentVerticalSpeed = Vector3.Dot(currentVelocity, motor.CharacterUp);
                var targetVerticalSpeed = Mathf.Max(currentVerticalSpeed, jumpSpeed);
                // Add the difference in current and target vertical speed to the character's velocity.
                currentVelocity += motor.CharacterUp * (targetVerticalSpeed - currentVerticalSpeed);
            }
            else
            {
                _requestedJump = false;
            }

        }

    }

    public void BeforeCharacterUpdate(float deltaTime)
    {
        _tempState = _state;

        // Crouch
        if (_requestedCrouch && _state.stance is Stance.Stand)
        {
            _state.stance = Stance.Crouch;
            motor.SetCapsuleDimensions
            (
                radius: motor.Capsule.radius,
                height: crouchHeight,
                yOffset: crouchHeight * 0.5f
            );
        }
    }

    public void AfterCharacterUpdate(float deltaTime)
    {
        // Uncrouch.
        if (!_requestedCrouch && _state.stance is not Stance.Stand)
        {
            motor.SetCapsuleDimensions
            (
                radius: motor.Capsule.radius,
                height: standHeight,
                yOffset: standHeight * 0.5f
            );

            // Determine if we are overlapping any colliders before actually standing up.
            var pos = motor.TransientPosition;
            var rot = motor.TransientRotation;
            var mask = motor.CollidableLayers;
            if (motor.CharacterOverlap(pos, rot, _uncrouchOverlapColliders, mask, QueryTriggerInteraction.Ignore) > 0)
            {
                // Re-crouch.
                _requestedCrouch = true;
                motor.SetCapsuleDimensions
                (
                    radius: motor.Capsule.radius,
                    height: crouchHeight,
                    yOffset: crouchHeight * 0.5f
                );
            }
            else
            {
                _state.stance = Stance.Stand;
            }
        }

        // Update state to reflect relevant motor properties.
        _state.Grounded = motor.GroundingStatus.IsStableOnGround;
        _state.Velocity = motor.Velocity;
        // And update the _lastState to store the character state snapshot taken at
        // the beginning of this character update.
        _lastState = _tempState;
    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        return true;
    }

    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {
    }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
    }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
    }

    public void PostGroundingUpdate(float deltaTime)
    {
        if (!motor.GroundingStatus.IsStableOnGround && _state.stance is Stance.Slide)
            _state.stance = Stance.Crouch;
    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {
    }

    public void SetPosition(Vector3 position, bool killVelocity = true)
    {
        motor.SetPosition(position);
        if (killVelocity)
            motor.BaseVelocity = Vector3.zero;
    }

    public void ToggleMovement(bool toggle)
    {
        canMove = toggle;
    }

    public void ToggleLook(bool toggle)
    {
        canLook = toggle;
    }

    public float HorizontalVelocityMagnitude()
    {
        return new Vector3(motor.Velocity.x, 0f, motor.Velocity.z).magnitude;
    }
}
