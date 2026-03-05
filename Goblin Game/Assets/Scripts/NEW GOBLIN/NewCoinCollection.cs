using UnityEngine;

public class NewCoinCollection : MonoBehaviour
{
    [SerializeField] PickupAction pickupAction;


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)     // 9-> Coin Layer
        {
            var coin = other.GetComponentInParent<Coin>();

            if(coin != null && pickupAction.HasJar)
            {
                CoinPool.Instance.OnCoinCollectedServerRpc(coin.ID);
                //pickupAction.CurrentPickup as JarPickup.GainCoin();
            }
        }
    }
}
