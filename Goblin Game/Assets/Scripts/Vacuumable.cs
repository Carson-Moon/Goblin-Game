using Unity.Netcode;
using UnityEngine;

public class Vacuumable : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;

    [Rpc(SendTo.Server)]
    public void ApplyForceToThisRPC(Vector3 force, Vector3 torque)
    {
        rb.AddForce(force);
        rb.AddTorque(torque);
    }
}
