using TMPro;
using UnityEngine;

public class GrabAction : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] Transform pickupPos;
    [SerializeField] GameObject heldJarVisual;
    [SerializeField] GameObject currentHeldVisual;

    [Header("Grab Detection Settings")]
    [SerializeField] Transform grabTransform;
    [SerializeField] float grabLength;
    [SerializeField] float grabRadius;
    [SerializeField] LayerMask pickupMask;
    [SerializeField] IPickup pickupCandidate;
    [SerializeField] IPickup currentPickup;
    public IPickup CurrentPickup => currentPickup;

    private JarPickup jarCandidate;
    [SerializeField] JarPickup currentJar;
    public JarPickup CurrentJar => currentJar;

    [Header("UI")]
    [SerializeField] GameObject jarCoinsUI;
    [SerializeField] TextMeshProUGUI jarCoinsDisplay;

    private Collider[] pickupCols;


    public void AttemptPickup()
    {
        if (pickupCandidate == null || currentPickup != null)
            return;

        pickupCandidate.OnPickup(pickupPos);
        currentHeldVisual = heldJarVisual;
        heldJarVisual.SetActive(true);

        currentPickup = pickupCandidate;
        currentJar = jarCandidate;
        pickupCandidate = null;
    }

    void Update()
    {
        CheckForPickups();

        if(currentJar != null)
        {
            jarCoinsUI.SetActive(true);
            jarCoinsDisplay.text = $"{currentJar.Coins}";
        }
        else
        {
            jarCoinsUI.SetActive(false);
        }
    }

    private void CheckForPickups()
    {
        pickupCols = Physics.OverlapCapsule(grabTransform.position, grabTransform.position + (grabTransform.forward * grabLength), grabRadius, pickupMask);

        if (pickupCols.Length > 0)
        {
            pickupCandidate = pickupCols[0].GetComponent<IPickup>();
            jarCandidate = pickupCols[0].GetComponent<JarPickup>();
        }
        else
        {
            pickupCandidate = null;
            jarCandidate = null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(grabTransform.position, grabTransform.position + (grabTransform.forward * grabLength));
    }

    public void DiscardCurrentPickup()
    {
        currentPickup = null;
        currentJar = null;
        currentHeldVisual.SetActive(false);
    }
}
