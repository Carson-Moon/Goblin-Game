using UnityEngine;
using Unity.Netcode;
using TMPro;

public class CoinManager_Goblin : NetworkBehaviour
{
    [Header("Current Coins")]
    [SerializeField] private int m_CurrentCoins = 0;
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] int coinLoseAmount;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
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
        if(m_CurrentCoins >= coinLoseAmount)
        {
            for(int i=0; i<coinLoseAmount; i++)
            {
                LoseCoinRPC();
                CreateCoinRPC();
            }
        }
        else
        {
            for(int i=0; i<m_CurrentCoins; i++)
            {
                LoseCoinRPC();
                CreateCoinRPC();
            }
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
        var instance = Instantiate(coinPrefab, transform.position + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f)), Quaternion.identity);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
    }

    void OnTriggerEnter(Collider other)
    {
        // If we collide with a coin, collect it!
        if(other.gameObject.layer == 9)
        {
            Coin coin = other.gameObject.GetComponentInParent<Coin>();

            if(coin.CanCollect())
            {
                GainCoin();

                coin.OnCollect();
            }
            
        }
    }
}
