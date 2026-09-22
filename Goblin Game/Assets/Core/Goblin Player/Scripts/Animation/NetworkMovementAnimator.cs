using System;
using KinematicCharacterController;
using Unity.Netcode;
using UnityEngine;

public class NetworkMovementAnimator : NetworkBehaviour
{
    [SerializeField] Transform forwardReference;
    [SerializeField] Animator animator;
    [SerializeField] KinematicCharacterMotor characterMotor;
    [SerializeField] GoblinCharacter goblinCharacter;

    [SerializeField] float maxMoveThreshold;
    [SerializeField] float smoothing;

    [SerializeField] NetworkVariable<Vector3> networkVelocity = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] NetworkVariable<bool> networkIsOnStableGround = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] NetworkVariable<bool> networkIsSliding = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private bool lastFrameIsOnStableGround = false;

    CharacterGroundingReport lastFrameGroundingStatus;
    Vector3 forwardRelativeVelocity;
    float horizontalSmoothVel;
    float forwardSmoothVel;
    float horizontalTarget;
    float forwardTarget;


    void Update()
    {
        if(IsOwner)
        {
            networkVelocity.Value = characterMotor.Velocity;
            networkIsOnStableGround.Value = characterMotor.GroundingStatus.IsStableOnGround;
            networkIsSliding.Value = goblinCharacter.Stance is Stance.Slide;
        }
            
        CheckForJump();
        CheckForLand();

        UpdateGroundAnimations();

        lastFrameIsOnStableGround = networkIsOnStableGround.Value;
    }

    public override void OnNetworkSpawn()
    {
        networkVelocity.OnValueChanged += OnVelocityChanged;
        networkIsOnStableGround.OnValueChanged += OnBoolChanged;
        networkIsSliding.OnValueChanged += OnBoolChanged;
        base.OnNetworkSpawn();
    }

    private void OnBoolChanged(bool previousValue, bool newValue)
    {
        
    }

    private void OnVelocityChanged(Vector3 previousValue, Vector3 newValue)
    {
        
    }

    private void UpdateGroundAnimations()
    {
        forwardRelativeVelocity = forwardReference.InverseTransformDirection(networkVelocity.Value);

        float xTarget = !networkIsSliding.Value ?Mathf.Clamp(forwardRelativeVelocity.x / maxMoveThreshold, -1, 1) : 0;
        float yTarget = !networkIsSliding.Value ? Mathf.Clamp(forwardRelativeVelocity.z / maxMoveThreshold, -1, 1) : 2;

        horizontalTarget = Mathf.SmoothDamp(horizontalTarget, xTarget, ref horizontalSmoothVel, smoothing);
        forwardTarget = Mathf.SmoothDamp(forwardTarget, yTarget, ref forwardSmoothVel, smoothing);

        animator.SetFloat("_Strafe", horizontalTarget);
        animator.SetFloat("_Forward", forwardTarget);
    }

    private void CheckForJump()
    {
        if(lastFrameIsOnStableGround && !networkIsOnStableGround.Value)
        {
            if(characterMotor.Velocity.y > 0)
                animator.SetTrigger("_Jump");
        }
    }

    private void CheckForLand()
    {
        if(!lastFrameIsOnStableGround && networkIsOnStableGround.Value)
        {
            animator.SetTrigger("_Land");
                
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void StabAnimationServerRpc()
    {
        StabAnimationClientRpc();
    }

    [ClientRpc]
    private void StabAnimationClientRpc()
    {
        animator.SetTrigger("stab");
    }
}
