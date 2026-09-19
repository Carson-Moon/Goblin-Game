using UnityEngine;
using KinematicCharacterController;

public class ViewBobbing : CameraEffect
{
    [Header("Character")]
    [SerializeField] private GoblinCharacter _character;
    [SerializeField] private KinematicCharacterMotor _motor;

    [Header("Gait")]
    [SerializeField] float frequency = 1.0f;

    [SerializeField] float horizontalAmplitude = 0.01f;
    [SerializeField] float verticalAmplitude = 0.015f;

    [Header("Rotation")]
    [SerializeField] float rollAmplitude = 0.35f;
    [SerializeField] float pitchAmplitude = 0.2f;

    [Header("Footstep Weight")]
    [SerializeField]
    private Spring stepSpring = new Spring(160f, 18f, 0.04f);

    [SerializeField] float stepForce = 0.15f;

    [Header("Transition")]
    [SerializeField] float blendSpeed = 8f;

    private float _phase;
    private float _weight;

    private float _speed;
    private bool _grounded;

    private Vector3 _positionOffset;
    private Vector3 _eulerOffset;

    // shorthand read-only 'gets' for the position and rotation offsets.
    // outside scripts can read this value, but cannot read or write to the fields.
    public override Vector3 PositionOffset => _positionOffset;

    public override Vector3 EulerOffset => _eulerOffset;

    public void Initialize(GoblinCharacter character, KinematicCharacterMotor motor)
    {
        _character = character;
        _motor = motor;
    }

    public override void Tick(float dt)
    {
        if (_character == null) return;

        float speed = _character.HorizontalVelocityMagnitude();
        bool grounded = _motor.GroundingStatus.IsStableOnGround;
        bool isSliding = _character.getStance() == Stance.Slide;

        bool isMoving = grounded && speed > 0.1f;

        float targetWeight = isMoving ? 1f : 0f;

        // Smoothly interpolate the weight towards the target weight. 
        // basically if the player is moving, we want to increase the weight of the bobbing effect, and if they are not moving, we want to decrease it over time.
        // This creates a smooth transition between the two states, rather than an abrupt change.
        _weight = Mathf.MoveTowards(_weight, targetWeight, blendSpeed * dt);

        // Detect footstep events.
        if (isMoving && !isSliding)
        {
            // If the player is moving and not sliding, we want to update the phase of the bobbing effect based on the speed and frequency of the movement.
            // the phase is used to give the appearance of left or right foot being planted.
            float previousPhase = _phase;

            _phase += speed * frequency * dt;

            DetectFootsteps(previousPhase, _phase);
        }

        stepSpring.Update(dt);

        // Calculate the position and rotation offsets based on the current phase of the bobbing effect.
        float horizontal = Mathf.Sin(_phase);

        float vertical = -Mathf.Cos(_phase * 2f);

        _positionOffset = new Vector3(horizontal * horizontalAmplitude, vertical * verticalAmplitude, 0f) * _weight;

        _positionOffset.y += stepSpring.Value * _weight;

        _eulerOffset = new Vector3(vertical * pitchAmplitude, 0f, -horizontal * rollAmplitude) * _weight;
    }


    // This method detects when a footstep occurs based on the phase of the bobbing effect.
    // It compares the previous phase and the current phase to determine if a footstep has occurred.
    // A footstep is considered to have occurred when the phase crosses a multiple of PI (i.e., when the left or right foot is planted).
    private void DetectFootsteps(float previousPhase, float currentPhase)
    {
        int previousStep = Mathf.FloorToInt(previousPhase / Mathf.PI);

        int currentStep = Mathf.FloorToInt(currentPhase / Mathf.PI);

        if (currentStep != previousStep)
        {
            OnFootstep();
        }
    }

    private void OnFootstep()
    {
        stepSpring.Nudge(-stepForce);
    }
}
