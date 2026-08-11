using UnityEngine;

public class UnconsciousTriggerObject : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered unconscious trigger!");
            UnconsciousManager unconsciousManager = other.GetComponent<UnconsciousManager>();
            if (unconsciousManager != null)
            {
                unconsciousManager.LoseConsciousness(this.transform.position);
            }
        }
    }
}
