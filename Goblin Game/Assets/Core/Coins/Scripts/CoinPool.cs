using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

// This manages all of our coin logic, spawning, despawning, etc.

public class CoinPool : NetworkBehaviour
{
    public static CoinPool Instance { get; private set; }

    [Header("Pool Settings")]
    [SerializeField] int numCoinsInPool = 10;
    [SerializeField] Coin coinPrefab;
    [SerializeField] Dictionary<int, Coin> localCoinPool = new();

    private int totalSpawnedCoins = 0;

    public bool debug;
    public Transform debugTransform;

    void Update()
    {
        if(debug)
        {
            debug = false;
            SpawnMultipleCoinsServerRpc(debugTransform.position, 1);
        }
    }


    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        SetupCoinPool();
    }

    public void SetupCoinPool()
    {
        for(int i=0; i<numCoinsInPool; i++)
        {
            Coin newCoin = Instantiate(coinPrefab, transform);
            newCoin.CreateCoin(i);
            localCoinPool.Add(i, newCoin);
        }
    }

    public Coin GetNextCoin(int? index)
    {
        if(index == null)
            return localCoinPool[totalSpawnedCoins++ % numCoinsInPool];
        else
            return localCoinPool[index.Value];
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void OnCoinCollectedServerRpc(int id)
    {
        ReturnCoinClientRpc(id);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void ReturnCoinClientRpc(int id)
    {
        localCoinPool[id].Deactivate();
    }

    [Rpc(SendTo.Server)]
    public void SpawnMultipleCoinsServerRpc(Vector3 spawnPosition, int numCoinsToSpawn)
    {
        for (int i = 0; i < numCoinsToSpawn; i++)
        {
            var coin = GetNextCoin(null);

            if(coin == null)
                return;

            Vector3 randomForce = Vector3.up + (Vector3.right * Random.Range(-1f, 1f)) + (Vector3.forward * Random.Range(-1f, 1f));
            
            coin.Activate(spawnPosition, randomForce);
            SpawnCoinClientRpc(coin.ID, spawnPosition, randomForce);
        }
    }

    [Rpc(SendTo.NotServer)]
    private void SpawnCoinClientRpc(int coinIndex, Vector3 _pos, Vector3 _force)
    {
        var coin = GetNextCoin(coinIndex);

        coin.Activate(_pos, _force);
    }
}
