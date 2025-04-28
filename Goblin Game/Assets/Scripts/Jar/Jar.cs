using Unity.Netcode;
using UnityEngine;

public class Jar : NetworkBehaviour
{
    // Runtime
    [SerializeField] Rigidbody rb;
    [SerializeField] Collider col;
    [SerializeField] Transform jarPosition;


    void Update()
    {
        if(jarPosition != null)
        {
            transform.position = jarPosition.position;
            transform.rotation = jarPosition.rotation;
        }
    }

    // Request ownership of this object.
    public void RequestOwnership()
    {
        if(!IsOwner)
        {
            RequestOwnershipServerRPC(NetworkManager.Singleton.LocalClientId);
        }
    }

    // Request ownership from the server.
    [Rpc(SendTo.Server)]
    public void RequestOwnershipServerRPC(ulong clientID)
    {
        NetworkObject.ChangeOwnership(clientID);
    }

    // Disable jar physics for pickup.
    public void DisablePhysics()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        col.enabled = false;
    }

    // Enable jar physics.
    public void EnablePhysics()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
        col.enabled = true;
    }

    // Apply force in direction.
    public void ImpulseInDirection(Vector3 direction, float strength)
    {
        rb.AddForce(strength * direction, ForceMode.Impulse);
    }

    // Set this jar position.
    public void SetJarPosition(Transform jarPosition)
    {
        this.jarPosition = jarPosition;
    }
}
