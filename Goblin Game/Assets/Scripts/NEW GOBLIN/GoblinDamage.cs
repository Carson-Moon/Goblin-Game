using Unity.Netcode;
using UnityEngine;

public class GoblinDamage : NetworkBehaviour, IDamageable
{
    ServerCoinManager ServerCoinManager => ServerCoinManager.Instance;

    [SerializeField] Transform coinSpawnPosition;
    [SerializeField] UnconsciousManager unconsciousManager;

    public void OnDeath()
    {

    }

    public void TakeDamage()
    {
        ServerCoinManager.SpawnMultipleCoinsServerRpc(coinSpawnPosition.position, 10);

        TakeDamageServerRpc();
    }

    [Rpc(SendTo.Server)]
    public void TakeDamageServerRpc()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 17) // Pickup layer
        {
            Debug.Log("Here");
            JarPickup jar = collision.gameObject.GetComponent<JarPickup>();
            if (jar != null && jar.Thrown)
            {
                Debug.Log("In here");
                unconsciousManager.GetKnockedOut();
            }
        }
    }
}
