using Unity.Netcode;
using UnityEngine;

public class BreakableSpawner : NetworkBehaviour
{
    [SerializeField] GameObject breakablePrefab;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if(IsServer)
        {
            // Create a new breakable.
            var instance = Instantiate(breakablePrefab, transform.position + new Vector3(0, 1,0), Quaternion.identity);
            var instanceNetworkObject = instance.GetComponent<NetworkObject>();
            instanceNetworkObject.Spawn();

            Destroy(gameObject);
        }        
    }
}
