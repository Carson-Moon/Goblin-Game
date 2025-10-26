using Unity.Netcode;
using UnityEngine;

// This manages all of our coin logic, spawning, despawning, etc.

public class ServerCoinManager : NetworkBehaviour
{
    public static ServerCoinManager Instance { get; private set; }

    [SerializeField] Coin coinPrefab;


    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    [Rpc(SendTo.Server)]
    public void SpawnMultipleCoinsServerRpc(Vector3 spawnPosition, int numCoinsToSpawn)
    {
        for (int i = 0; i < numCoinsToSpawn; i++)
        {
            var instance = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
            instance.GetComponent<NetworkObject>().Spawn();
        }
    }
}
