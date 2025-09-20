using Unity.Netcode;
using UnityEngine;

public class GoblinDamage : NetworkBehaviour, IDamageable
{
    ServerCoinManager ServerCoinManager => ServerCoinManager.Instance;

    [SerializeField] Transform coinSpawnPosition;

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
}
