using KinematicCharacterController;
using UnityEngine;

public class NetworkMovementAnimator : MonoBehaviour
{
    [SerializeField] Transform forwardReference;
    [SerializeField] Animator animator;
    [SerializeField] KinematicCharacterMotor characterMotor;

    [SerializeField] float maxMoveThreshold;
    [SerializeField] float smoothing;

    Vector3 lastFrameVelocity;
    CharacterGroundingReport lastFrameGroundingStatus;
    Vector3 forwardRelativeVelocity;
    float horizontalSmoothVel;
    float forwardSmoothVel;
    float horizontalTarget;
    float forwardTarget;


    void Update()
    {
        CheckForJump();
        CheckForLand();

        UpdateGroundAnimations();

        lastFrameVelocity = characterMotor.Velocity;
        lastFrameGroundingStatus = characterMotor.GroundingStatus;
    }

    private void UpdateGroundAnimations()
    {
        forwardRelativeVelocity = forwardReference.InverseTransformDirection(characterMotor.Velocity);

        horizontalTarget = Mathf.SmoothDamp(horizontalTarget, Mathf.Clamp(forwardRelativeVelocity.x / maxMoveThreshold, -1, 1), ref horizontalSmoothVel, smoothing);
        forwardTarget = Mathf.SmoothDamp(forwardTarget, Mathf.Clamp(forwardRelativeVelocity.z / maxMoveThreshold, -1, 1), ref forwardSmoothVel, smoothing);

        animator.SetFloat("_Strafe", horizontalTarget);
        animator.SetFloat("_Forward", forwardTarget);
    }

    private void CheckForJump()
    {
        if(lastFrameGroundingStatus.IsStableOnGround && !characterMotor.GroundingStatus.IsStableOnGround)
        {
            if(characterMotor.Velocity.y > 0)
                animator.SetTrigger("_Jump");
        }
    }

    private void CheckForLand()
    {
        if(!lastFrameGroundingStatus.IsStableOnGround && characterMotor.GroundingStatus.IsStableOnGround)
        {
            animator.SetTrigger("_Land");
                
        }
    }
}
