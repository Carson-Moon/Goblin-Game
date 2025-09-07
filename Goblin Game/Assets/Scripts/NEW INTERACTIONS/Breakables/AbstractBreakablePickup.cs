using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AbstractBreakablePickup : MonoBehaviour, IPickup
{
    [Header("Breakable Pickup Components")]
    [SerializeField] Rigidbody rb;
    [SerializeField] bool thrown;
    public bool Thrown => thrown;

    [Header("FX")]
    [SerializeField] GameObject onBreakVFX;
    [SerializeField] AudioClip onBreakSFX;


    public virtual void OnPickup(Transform pickupPos)
    {
        DisablePhysics();
        ParentToPickupPosition(pickupPos);
    }

    public virtual void OnThrow(Vector3 throwDirection, float throwForce)
    {
        UnparentFromPickupPosition();

        EnablePhysics();

        rb.AddForce(throwForce * throwDirection, ForceMode.Impulse);

        thrown = true;
    }

    public virtual void Break()
    {
        onBreakVFX.transform.SetParent(null);
        onBreakVFX.SetActive(true);
        Destroy(onBreakVFX, 5f);

        Destroy(gameObject);
    }

    public void ParentToPickupPosition(Transform pickupPos)
    {
        transform.SetParent(pickupPos);
        transform.localPosition = Vector3.zero;
    }

    public void UnparentFromPickupPosition()
    {
        transform.SetParent(null);
    }

    public void DisablePhysics()
    {
        gameObject.SetActive(false);
        rb.isKinematic = true;
    }

    public void EnablePhysics()
    {
        gameObject.SetActive(true);
        rb.isKinematic = false;
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (thrown)
            Break();
    }
}
