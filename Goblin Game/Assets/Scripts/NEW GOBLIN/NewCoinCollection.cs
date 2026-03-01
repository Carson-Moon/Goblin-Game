using UnityEngine;

public class NewCoinCollection : MonoBehaviour
{
    [SerializeField] PickupAction pickupAction;


    void OnTriggerEnter(Collider other)
    {
        // if(pickupAction.CurrentJar == null)
        //     return;

        if(other.gameObject.layer == 9)     // 9-> Coin Layer
        {
            other.GetComponentInParent<Coin>().OnCollect();

            // pickupAction.CurrentJar.GainCoin();
        }
    }
}
