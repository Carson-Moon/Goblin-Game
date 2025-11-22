using Unity.Netcode;
using UnityEngine;

public class GoblinDamage : NetworkBehaviour, IDamageable
{
    ServerCoinManager ServerCoinManager => ServerCoinManager.Instance;

    [SerializeField] Transform coinSpawnPosition;
    [SerializeField] UnconsciousManager unconsciousManager;
    [SerializeField] GoblinCoinEating goblinCoinEating;

    public void OnDeath()
    {

    }

    public void TakeDamage(Vector3 damagePoint)
    {
        Debug.Log("Take damage");

        int coinsToLose = goblinCoinEating.SubtractFromCoinsEaten(5);
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
            JarPickup jar = collision.gameObject.GetComponent<JarPickup>();
            if (jar != null && jar.Thrown)
            {
                unconsciousManager.GetKnockedOut();
            }
        }
    }
}
