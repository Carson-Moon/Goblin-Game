using Unity.Netcode;
using UnityEngine;

public class GoblinDamage : NetworkBehaviour, IDamageable
{
    ServerCoinManager ServerCoinManager => ServerCoinManager.Instance;

    [SerializeField] Transform coinSpawnPosition;
    [SerializeField] UnconsciousManager unconsciousManager;
    private GoblinCoins goblinCoins;


    void Awake()
    {
        goblinCoins = GetComponent<GoblinCoins>();
    }

    public void TakeDamage(Vector3 damagePoint)
    {
        int coinsToLose = goblinCoins.LoseCoins(5);

        if(ServerCoinManager != null)
            ServerCoinManager.SpawnMultipleCoinsServerRpc(coinSpawnPosition.position, coinsToLose);

        TakeDamageServerRpc();
    }

    [Rpc(SendTo.Server)]
    public void TakeDamageServerRpc()
    {

    }

    public void OnDeath()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if(!IsOwner)
            return;

        if(collision.gameObject.TryGetComponent<Pickup>(out Pickup pickup) && pickup.Thrown)
            unconsciousManager.LoseConsciousness(collision.transform.position);
    }
}
