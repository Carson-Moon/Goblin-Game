using Unity.Netcode;
using UnityEngine;

public class Breakable : NetworkBehaviour, IDamageable
{
    [SerializeField] int health;
    [SerializeField] int coinAmount;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject cratePiecesPrefab;
    [SerializeField] Transform centerPoint;
    [SerializeField] Collider col;
    [SerializeField] MeshRenderer meshRen;


    public void TakeDamage(Vector3 damagePoint)
    {
        if(health == 1)
        {
            CreateCoinsRPC();

            BreakRPC(damagePoint);
        }
        else
        {
            TakeDamageRPC();
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void TakeDamageRPC()
    {
        health--;
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    private void BreakRPC(Vector3 damagePoint)
    {
        ActivateBreakablesRPC(damagePoint);

        GetComponent<NetworkObject>().Despawn(true);
    }

    [Rpc(SendTo.Server)]
    private void ActivateBreakablesRPC(Vector3 damagePoint)
    {
        col.enabled = false;
        meshRen.enabled = false;

        var instance = Instantiate(cratePiecesPrefab, transform.position, Quaternion.identity);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
        instance.GetComponent<BreakableExplosion>().ExplodeRPC(damagePoint);
    }

    [Rpc(SendTo.Server)]
    private void CreateCoinsRPC()
    {
        for(int i=0; i<coinAmount; i++)
        {
            // Instantiate a new coin.
            var instance = Instantiate(coinPrefab, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-0.5f, 0.5f), Random.Range(-1f, 1f)), Quaternion.identity);
            var instanceNetworkObject = instance.GetComponent<NetworkObject>();
            instanceNetworkObject.Spawn();
        }       
    }

    public void OnDeath()
    {
        
    }
}
