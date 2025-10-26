using UnityEngine;

public class ThrowAction : MonoBehaviour
{
    [SerializeField] GrabAction grabAction;

    [Header("Throw Settings")]
    [SerializeField] Camera playerCamera;
    [SerializeField] float targetDistance;
    [SerializeField] float throwStrength;
    [SerializeField] Transform throwStartPosition;

    public void AttemptThrow()
    {
        if (grabAction.CurrentPickup == null)
            return;

        Rigidbody throwRB = grabAction.CurrentPickup.GetRigidbody();

        Vector3 throwDirection = playerCamera.transform.forward * targetDistance;

        grabAction.CurrentPickup.OnThrow(throwStartPosition.position, throwDirection, throwStrength);
        grabAction.DiscardCurrentPickup();
    }
}
