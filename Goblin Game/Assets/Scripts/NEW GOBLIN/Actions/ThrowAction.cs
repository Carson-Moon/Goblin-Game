using UnityEngine;

public class ThrowAction : MonoBehaviour
{
    [SerializeField] GrabAction grabAction;

    [Header("Throw Settings")]
    [SerializeField] Camera playerCamera;
    [SerializeField] float targetDistance;
    [SerializeField] float throwStrength;

    public void AttemptThrow()
    {
        if (grabAction.CurrentPickup == null)
            return;

        Rigidbody throwRB = grabAction.CurrentPickup.GetRigidbody();

        Vector3 throwDirection = playerCamera.transform.forward * targetDistance;

        grabAction.CurrentPickup.OnThrow(throwDirection, throwStrength);
        grabAction.DiscardCurrentPickup();
    }
}
