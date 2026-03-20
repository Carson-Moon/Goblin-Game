using UnityEngine;

public class ThrowAction : MonoBehaviour
{
    [SerializeField] PickupAction _pickupAction;
    [SerializeField] PickupVisuals _pickupVisuals;
    [SerializeField] PickupVisualUI _pickupUI;

    [Header("Throw Settings")]
    [SerializeField] Camera playerCamera;
    [SerializeField] float targetDistance;
    [SerializeField] float throwStrength;
    [SerializeField] Transform throwStartPosition;

    public void AttemptThrow()
    {
        if (_pickupAction.CurrentPickup == null)
            return;

        // Rigidbody throwRB = pickupAction.CurrentPickup.GetRigidbody();

        Vector3 throwDirection = playerCamera.transform.forward * targetDistance;

        _pickupAction.CurrentPickup.OnThrow(throwStartPosition.position, throwStartPosition.rotation, throwDirection, throwStrength);
        _pickupAction.DiscardCurrentPickup();
        _pickupUI.OffPickup();

        _pickupVisuals.TogglePickupVisualClientRpc(PickupID.None);
    }
}
