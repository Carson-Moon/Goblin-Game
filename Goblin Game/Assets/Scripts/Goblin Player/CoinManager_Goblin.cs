using UnityEngine;
using Unity.Netcode;
using TMPro;

public class CoinManager_Goblin : NetworkBehaviour
{
    [Header("Current Coins")]
    [SerializeField] private int m_CurrentCoins = 0;
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] GameObject coinPrefab;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        print(IsOwner);
    }

    public void GainCoin()
    {
        GainCoinRPC();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void GainCoinRPC()
    {       
        m_CurrentCoins++;

        if(IsOwner)
        {
            print("Just gained a coin!");

            coinsText.text = m_CurrentCoins.ToString();
        }
    }

    public void LoseCoin()
    {
        if(m_CurrentCoins > 0)
        {
            LoseCoinRPC();
            CreateCoinRPC();
        }       
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void LoseCoinRPC()
    {
        m_CurrentCoins--;

        if(IsOwner)
        {
            print("Just lost a coin...");

            coinsText.text = m_CurrentCoins.ToString();
        }
    }

    [Rpc(SendTo.Server)]
    private void CreateCoinRPC()
    {
        // Instantiate a new coin.
        var instance = Instantiate(coinPrefab, transform.position + new Vector3(0, 1,0), Quaternion.identity);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
    }

    void OnCollisionEnter(Collision collision)
    {
        // If we collide with a coin, collect it!
        if(collision.gameObject.layer == 9)
        {
            Coin coin = collision.gameObject.GetComponent<Coin>();

            if(coin.CanCollect())
            {
                GainCoin();

                coin.OnCollect();
            }
            
        }
    }
}
