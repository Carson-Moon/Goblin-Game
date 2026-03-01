using TMPro;
using UnityEngine;

public class PickupAction : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] Transform pickupTransform;
    [SerializeField] float pickupLength;
    [SerializeField] float pickupRadius;
    [SerializeField] LayerMask pickupMask;
    [SerializeField] private Pickup _pickupCandidate;
    [SerializeField] private Pickup _currentPickup;
    public Pickup CurrentPickup => _currentPickup;


    // [Header("UI")]
    // [SerializeField] GameObject jarCoinsUI;
    // [SerializeField] TextMeshProUGUI jarCoinsDisplay;

    private Collider[] pickupCols;


    public void AttemptPickup()
    {
        if (_pickupCandidate == null || _currentPickup != null)
            return;

        _pickupCandidate.OnPickup();

        _currentPickup = _pickupCandidate;
        _pickupCandidate = null;
    }

    void Update()
    {
        CheckForPickups();
    }

    private void CheckForPickups()
    {
        pickupCols = Physics.OverlapCapsule(pickupTransform.position, pickupTransform.position + (pickupTransform.forward * pickupLength), pickupRadius, pickupMask);

        if (pickupCols.Length > 0)
        {
            _pickupCandidate = pickupCols[0].GetComponent<Pickup>();
        }
        else
        {
            _pickupCandidate = null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(pickupTransform.position, pickupTransform.position + (pickupTransform.forward * pickupLength));
    }

    public void DiscardCurrentPickup()
    {
        _currentPickup = null;
    }
}
