using UnityEngine;

public interface IPickup
{
    public void OnPickup(Transform pickupPos);
    public void OnThrow(Vector3 throwStartPos, Vector3 throwDirection, float throwForce);
    public void ParentToPickupPosition(Transform pickupPos);
    public void UnparentFromPickupPosition();
    public void DisablePhysics();
    public void EnablePhysics();
    public Rigidbody GetRigidbody();
}
