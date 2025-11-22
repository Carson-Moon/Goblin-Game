using UnityEngine;

public class NewCoinCollection : MonoBehaviour
{
    [SerializeField] GrabAction grabAction;


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)     // 9-> Coin Layer
        {
            other.GetComponentInParent<Coin>().OnCollect();

            grabAction.CurrentJar.GainCoin();
        }
    }
}
