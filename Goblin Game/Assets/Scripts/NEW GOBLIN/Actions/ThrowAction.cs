using UnityEngine;

public class ThrowAction : MonoBehaviour
{
    [SerializeField] PickupAction _pickupAction;
    [SerializeField] PickupVisuals _pickupVisuals;
    [SerializeField] PickupVisualUI _pickupUI;
    [SerializeField] CameraZoom cameraZoom;
    [SerializeField] TrajectoryVisualizer trajectoryVisualizer;

    [Header("Throw Settings")]
    [SerializeField] Camera playerCamera;
    [SerializeField] float throwChargeLength;
    [SerializeField] float targetDistance;
    [SerializeField] float throwStrength;
    [SerializeField] Transform throwStartPosition;


    public void ChargeThrow()
    {
        if(_pickupAction.CurrentPickup == null)
            return;
        
        cameraZoom.StartZoomIn(throwChargeLength);
        trajectoryVisualizer.StartVisualizing(throwStrength, _pickupAction.CurrentPickup.GetComponent<Rigidbody>().mass);
    }

    public void AttemptThrow()
    {
        cameraZoom.StopZoomIn();
        trajectoryVisualizer.StopVisualizing();

        if (_pickupAction.CurrentPickup == null)
            return;

        // Rigidbody throwRB = pickupAction.CurrentPickup.GetRigidbody();

        Vector3 throwDirection = playerCamera.transform.forward;

        _pickupAction.CurrentPickup.OnThrow(throwStartPosition.position, throwStartPosition.rotation, throwDirection, throwStrength);
        _pickupAction.DiscardCurrentPickup();
        _pickupUI.OffPickup();

        _pickupVisuals.TogglePickupVisualClientRpc(PickupID.None);
    }
}
