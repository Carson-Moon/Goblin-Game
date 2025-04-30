using Unity.Netcode;
using UnityEngine;

public class Breakable : NetworkBehaviour
{
    [SerializeField] int health;
    [SerializeField] int coinAmount;
    [SerializeField] GameObject coinPrefab;


    public void TakeDamage()
    {
        if(health == 1)
        {
            CreateCoinsRPC();

            BreakRPC();
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

    [Rpc(SendTo.Server)]
    private void BreakRPC()
    {
        GetComponent<NetworkObject>().Despawn(true);
    }

    [Rpc(SendTo.Server)]
    private void CreateCoinsRPC()
    {
        for(int i=0; i<coinAmount; i++)
        {
            // Instantiate a new coin.
            var instance = Instantiate(coinPrefab, transform.position + new Vector3(0, 1,0), Quaternion.identity);
            var instanceNetworkObject = instance.GetComponent<NetworkObject>();
            instanceNetworkObject.Spawn();
        }       
    }
}
