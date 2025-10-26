using UnityEngine;

public class GrabAction : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] Transform pickupPos;
    [SerializeField] GameObject heldJarVisual;
    [SerializeField] GameObject currentHeldVisual;

    [Header("Grab Detection Settings")]
    [SerializeField] Transform grabTransform;
    [SerializeField] float grabRadius;
    [SerializeField] LayerMask pickupMask;
    [SerializeField] IPickup pickupCandidate;
    [SerializeField] IPickup currentPickup;
    public IPickup CurrentPickup => currentPickup;

    private Collider[] pickupCols;


    public void AttemptPickup()
    {
        Debug.Log("Attempting Pickup!");

        if (pickupCandidate == null || currentPickup != null)
            return;

        Debug.Log("Successful Pickup!");

        pickupCandidate.OnPickup(pickupPos);
        currentHeldVisual = heldJarVisual;
        heldJarVisual.SetActive(true);

        currentPickup = pickupCandidate;
        pickupCandidate = null;
    }

    void Update()
    {
        CheckForPickups();
    }

    private void CheckForPickups()
    {
        pickupCols = Physics.OverlapSphere(grabTransform.position, grabRadius, pickupMask);

        if (pickupCols.Length > 0)
        {
            pickupCandidate = pickupCols[0].GetComponent<IPickup>();
            Debug.Log($"Pickup Candidate: {pickupCols[0].name}");
        }
        else
        {
            pickupCandidate = null;
        }
    }

    public void DiscardCurrentPickup()
    {
        currentPickup = null;
        currentHeldVisual.SetActive(false);
    }
}
