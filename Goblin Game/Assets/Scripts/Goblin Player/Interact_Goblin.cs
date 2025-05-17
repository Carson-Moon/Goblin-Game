using UnityEngine;

public class Interact_Goblin : MonoBehaviour
{
    // Runtime
    [SerializeField] Camera m_Camera;
    private RaycastHit m_Hit;

    [Header("Detection Settings")]
    [SerializeField] LayerMask interactLayer;
    [SerializeField] float rayLength;
    [SerializeField] GameObject detectedInteractable;

    [Header("UI")]
    [SerializeField] GameObject interactionIndicator;


    void Update()
    {
        // Perform our interactable raycast.
        DetectInteractable(); 
    }

    // Grabs a reference to an interactable if we are detecting one.
    private void DetectInteractable()
    {
        if(Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out m_Hit, rayLength, interactLayer))
        {
            detectedInteractable = m_Hit.transform.gameObject;

            interactionIndicator.SetActive(true);
        }
        else
        {
            detectedInteractable = null;

            interactionIndicator.SetActive(false);
        }
    }

    // Attempt to interact.
    public void AttemptInteraction()
    {
        if (detectedInteractable == null)
            return;
        
        // Perform interactable things.
    }
}
