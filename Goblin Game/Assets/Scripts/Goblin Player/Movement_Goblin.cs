using UnityEngine;

public class Movement_Goblin : MonoBehaviour
{
    // Runtime
    Rigidbody rb;
    Input_Goblin gInput;
    EnvironmentChecks_Goblin eChecks;

    [Header("Movement Toggles")]
    [SerializeField] bool canMove = true;
    [SerializeField] bool canJump = true;
    [SerializeField] bool canCrouch = true;

    [Header("Movement Tweaks")]
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;
    [SerializeField] float baseMaxSpeed;
    [SerializeField] float sprintMaxSpeed;
    [SerializeField] float airMultiplier;
    [SerializeField] float crouchMultiplier;
    [SerializeField] Transform orientation;

    [Header("Physics Tweaks")]
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;
    [SerializeField] float extraGravity;
    [SerializeField] float lowJumpGravity;

    [Header("Jump Tweaks")]
    [SerializeField] float jumpForce;
    [SerializeField] int airJumpsTotal;
    [SerializeField] int airJumps;
    bool isJumping = false;

    [Header("Crouch Tweaks")]
    [SerializeField] bool attemptingCrouch = false;
    [SerializeField] bool isCrouching = false;
    [SerializeField] CapsuleCollider bodyCollider;
    [SerializeField] Transform goblinGraphics;
    [SerializeField] Transform cameraTransform;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gInput = GetComponent<Input_Goblin>();
        eChecks = GetComponent<EnvironmentChecks_Goblin>();

        // Initialize number of Jumps.
        airJumps = airJumpsTotal;
    }

    private void Update()
    {
        // Update Drag
        UpdateDrag();
            
        // Move player if possible.
        if(canMove)
        {
            MovePlayer();
        }     

        // Apply extra gravity if we are falling.
        if(rb.linearVelocity.y < 0 && !eChecks.IsGrounded())
        {
            ExtraFallingGravity();
        }
  
        // Apply low-jump gravity.
        if(rb.linearVelocity.y > 0 && !isJumping)
        {
            LowJumpGravity();
        }

        // Attempt to uncrouch if we are not tryig to crouch and can stand!
        if(isCrouching && !attemptingCrouch && eChecks.CanStand())
        {
            Uncrouch();
        }
    }

#region Movement
    // Responsible for moving the player when we have input.
    private void MovePlayer()
    {
        // Fix the move direction to be relative to camera, and x, z.
        Vector3 fixedMoveDir = FixMoveDirection(gInput.GetRawMoveDirection());
        Vector3 moveForce = fixedMoveDir * acceleration;

        // Clamp our move force.
        moveForce = ClampMoveForce(moveForce);

        // Add force in the direction of movement.
        if(isCrouching)
        {
            rb.AddForce(moveForce * crouchMultiplier * Time.deltaTime);
        }
        else if(eChecks.IsGrounded())
        {
            rb.AddForce(moveForce * Time.deltaTime);
        }
        else
        {
            rb.AddForce(moveForce * airMultiplier * Time.deltaTime);
        }
    }

    // Takes in our raw vector2 input and spits out a vector3 for our 3D movement.
    private Vector3 FixMoveDirection(Vector2 dir)
    {
        // Multiply the orientations forward and right directions by move direction to fix input!
        return orientation.forward * dir.y + orientation.right * dir.x;
    }

    // Determine the force we should apply to the player. Do not exceed max speed.
    private Vector3 ClampMoveForce(Vector3 initial)
    {
        // Get our horizontal velocity.
        Vector3 horizVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        // If we are going to exceed our max speed, cut down on that velocity.
        float sqrMaxSpeed = maxSpeed * maxSpeed;
        bool maxSpeedExceeded = horizVelocity.sqrMagnitude > sqrMaxSpeed;

        if(maxSpeedExceeded)
        {
            // Subtract the component of the force vector that is in the direction of motion.
            // thus, allowing us to only apply forces that won't exceed our max speed.
            Vector3 forwardComponent = Vector3.Project(initial, horizVelocity);
            initial -= forwardComponent;
        }

        return initial;
    }
#endregion

#region Jump
    // Starting a jump!
    public void OnJump()
    {
        if(!canJump)
        {
            return;
        }

        // If we are grounded, or we are in the air and have an air jump.
        if(eChecks.IsGrounded())
        {
            ApplyJumpForce();

        }
        else if(!eChecks.IsGrounded() && airJumps > 0)
        {
            ApplyJumpForce();
            airJumps--;

        }            
    }

    // Perform the jump.
    private void ApplyJumpForce()
    {
        // Zero out y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        // Apply jump force.
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        isJumping = true;
    }

    // Stopping a jump!
    public void OffJump()
    {
        isJumping = false;
    }
#endregion

#region Crouch
    // Start crouching.
    public void OnCrouch()
    {
        attemptingCrouch = true;
        isCrouching = true;

        // Shrink our collider height.
        bodyCollider.height = 1f;
        bodyCollider.center = new Vector3(0, -0.5f, 0);

        goblinGraphics.localScale = new Vector3(1, 0.5f, 1);
        goblinGraphics.localPosition = new Vector3(0, -0.5f, 1);
        cameraTransform.localPosition = new Vector3(0, -.25f, 0);
    }

    // Stop saying we are trying to crouch.
    public void OffCrouch()
    {
        attemptingCrouch = false;
    }

    // Stop crouching.
    private void Uncrouch()
    {
        isCrouching = false;

        // Reset collider height.
        bodyCollider.height = 2f;
        bodyCollider.center = Vector3.zero;

        goblinGraphics.localScale = new Vector3(1, 1, 1);
        goblinGraphics.localPosition = new Vector3(0, 0, 1);
        cameraTransform.localPosition = new Vector3(0, .75f, 0);
    }
#endregion

    #region Gravity
    // Apply extra gravity when we are falling.
    private void ExtraFallingGravity()
    {
        rb.AddForce(-transform.up * extraGravity * Time.deltaTime);
    }

    // Apply extra gravity when we stop jumping.
    private void LowJumpGravity()
    {
        rb.AddForce(-transform.up * lowJumpGravity * Time.deltaTime);
    }
#endregion

#region Drag Control
    // Update our drag depending on if we are grounded or not.
    private void UpdateDrag()
    {
        if(eChecks.IsGrounded())
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = airDrag;
        }
    }

#endregion

#region Sprint
    // Start sprinting.
    public void OnSprint()
    {
        maxSpeed = sprintMaxSpeed;
    }

    // Stop sprinting.
    public void OffSprint()
    {
        maxSpeed = baseMaxSpeed;
    }
#endregion
}
