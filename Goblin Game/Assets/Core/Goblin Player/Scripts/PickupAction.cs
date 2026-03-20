using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PickupAction : MonoBehaviour
{
    [SerializeField] PickupVisuals _pickupVisuals;
    [SerializeField] PickupVisualUI _pickupUI;

    [Header("Detection Settings")]
    [SerializeField] Transform pickupTransform;
    [SerializeField] float pickupLength;
    [SerializeField] float pickupRadius;
    [SerializeField] LayerMask pickupMask;
    private Pickup _pickupCandidate;
    private Collider[] pickupCols;

    private Pickup _currentPickup;
    public Pickup CurrentPickup => _currentPickup;



    public void AttemptPickup()
    {
        if (_pickupCandidate == null || _currentPickup != null)
            return;

        _pickupCandidate.OnPickup();

        _pickupVisuals.TogglePickupVisualClientRpc(_pickupCandidate.ID);
        _pickupUI.OnPickup(_pickupCandidate);

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
