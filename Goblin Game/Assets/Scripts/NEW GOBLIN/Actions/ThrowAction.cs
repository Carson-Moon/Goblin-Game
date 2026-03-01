using UnityEngine;

public class ThrowAction : MonoBehaviour
{
    [SerializeField] PickupAction pickupAction;

    [Header("Throw Settings")]
    [SerializeField] Camera playerCamera;
    [SerializeField] float targetDistance;
    [SerializeField] float throwStrength;
    [SerializeField] Transform throwStartPosition;

    public void AttemptThrow()
    {
        if (pickupAction.CurrentPickup == null)
            return;

        // Rigidbody throwRB = pickupAction.CurrentPickup.GetRigidbody();

        Vector3 throwDirection = playerCamera.transform.forward * targetDistance;

        pickupAction.CurrentPickup.OnThrow(throwStartPosition.position, throwDirection, throwStrength);
        pickupAction.DiscardCurrentPickup();
    }
}
