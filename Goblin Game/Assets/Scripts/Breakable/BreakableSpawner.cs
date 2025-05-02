using Unity.Netcode;
using UnityEngine;

public class BreakableSpawner : NetworkBehaviour
{
    [SerializeField] GameObject breakablePrefab;
    [SerializeField, Tooltip("Ghost object is exactly what the object will look like. Used to visualize before the object is spawned.")]
    private GameObject ghostObject;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Set this ghost object to false.
        ghostObject.SetActive(false);

        if(IsServer)
        {
            // Create a new breakable.
            var instance = Instantiate(breakablePrefab, transform.position, Quaternion.identity);
            var instanceNetworkObject = instance.GetComponent<NetworkObject>();
            instanceNetworkObject.Spawn();
        }
    }
}
