using UnityEngine;

public class EnvironmentChecks_Goblin : MonoBehaviour
{
    [Header("Check Bools")]
    [SerializeField] bool isGrounded;
    [SerializeField] bool canStand;

    [Header("Shared Settings")]
    [SerializeField] LayerMask environmentLayers;

    [Header("Ground Check")]
    [SerializeField] float groundCheckRadius;
    [SerializeField] Vector3 groundCheckOffset;

    [Header("Ceiling Check")]
    [SerializeField] float ceilingCheckRadius;
    [SerializeField] Vector3 ceilingCheckOffset;
    


    void Update()
    {
        GroundCheck();
        CeilingCheck();
    }

    // Ground Check...
    public bool GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.position + groundCheckOffset, groundCheckRadius, environmentLayers);
        return isGrounded;
    }

    // Ceiling Check...
    public bool CeilingCheck()
    {
        canStand = !Physics.CheckSphere(transform.position + ceilingCheckOffset, ceilingCheckRadius, environmentLayers);
        return canStand;
    }

#region Getters
    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool CanStand()
    {
        return canStand;
    }
#endregion
}
