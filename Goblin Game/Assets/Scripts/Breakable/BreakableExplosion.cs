using Unity.Netcode;
using UnityEngine;

public class BreakableExplosion : NetworkBehaviour
{
    [SerializeField] Rigidbody[] pieces;
    [SerializeField] float explosionForce;


    [Rpc(SendTo.Server)]
    public void ExplodeRPC(Vector3 origin)
    {
        // Explode away from the origin.
        int count = pieces.Length;
        for(int i=0; i<count; i++)
        {
            Vector3 direction = (pieces[i].position - origin).normalized;
            pieces[i].AddForce(direction * explosionForce, ForceMode.Impulse);
        }
    }
}
