using Unity.Netcode;
using UnityEngine;

public class Vacuumable : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    public void ApplyForceToThis(Vector3 force, Vector3 torque)
    {
        rb.AddForce(force);
        rb.AddTorque(torque);
    }
}
