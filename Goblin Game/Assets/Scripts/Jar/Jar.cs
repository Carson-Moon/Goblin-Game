using Unity.Netcode;
using UnityEngine;

public class Jar : NetworkBehaviour
{
    // Runtime
    [SerializeField] Rigidbody rb;
    [SerializeField] Collider col;
    [SerializeField] bool canStun = false;
    [SerializeField] bool canBreak = false;
    

    // Request ownership of this object.
    public void RequestOwnership()
    {
        if (!IsOwner)
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

    void OnCollisionEnter(Collision collision)
    {
        // Collisions do not matter unless we can stun or break.
        if (canStun || canBreak)
        {
            // If we hit a goblin, do not break!
            if (collision.gameObject.layer == 7)
            {
                DisableStun();
            }

            // If we hit the lid, do nothing.
            else if (collision.gameObject.layer == 15)
            {
                // Do nothing.
            }
            
            // If we hit anything else, shatter and throw coins everywhere!
            else
            {
                DespawnJarRPC();
            }
        }  
    }


    public void EnableStun()
    {
        EnableStunRPC();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void EnableStunRPC()
    {
        canStun = true;
        canBreak = true;
    }

    public void DisableStun()
    {
        DisableStunRPC();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void DisableStunRPC()
    {
        canStun = false;
        canBreak = false;
    }

    public bool CanStun()
    {
        return canStun;
    }
    
    [Rpc(SendTo.Server)]
    private void DespawnJarRPC()
    {
        GetComponentInParent<NetworkObject>().Despawn();
    }
}
