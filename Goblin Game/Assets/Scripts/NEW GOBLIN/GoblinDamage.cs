using Unity.Netcode;
using UnityEngine;

public class GoblinDamage : NetworkBehaviour, IDamageable
{
    [SerializeField] DamageVignette damageVignette;
    [SerializeField] CameraShake cameraShake;

    [SerializeField] ImpulseController impulseController;
    [SerializeField] Transform coinSpawnPosition;
    [SerializeField] UnconsciousManager unconsciousManager;
    [SerializeField] GoblinCoins goblinCoins;


    public void TakeDamage(Vector3 damagePoint)
    {
        Debug.Log("Take Damage.");
        TakeDamageClientRpc(damagePoint);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void TakeDamageClientRpc(Vector3 damagePoint)
    {
        if(!IsOwner)
            return;

        if(RoundStatTracker.Instance != null)
            RoundStatTracker.Instance.TrackIntStat(IntStat.Got_Stabbed);

        damageVignette.PerformDamageFlash();
        cameraShake.PerformImpulseShake();
        impulseController.AddImpulse(((transform.position - damagePoint) + Vector3.up).normalized, 30);

        int coinsToLose = goblinCoins.LoseCoins(5);

        if(CoinPool.Instance != null)
            CoinPool.Instance.SpawnMultipleCoinsServerRpc(coinSpawnPosition.position, coinsToLose);
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
