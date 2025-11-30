using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AbstractBreakablePickup : NetworkBehaviour, IPickup
{
    [Header("Breakable Pickup Components")]
    [SerializeField] Rigidbody rb;
    [SerializeField] bool thrown;
    public bool Thrown => thrown;
    private bool broken = false;

    [Header("FX")]
    [SerializeField] GameObject onBreakVFX;
    [SerializeField] AudioClip onBreakSFX;


    public virtual void OnPickup(Transform pickupPos)
    {
        OnPickupClientRpc();
        //ParentToPickupPosition(pickupPos);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void OnPickupClientRpc()
    {
        DisablePhysics();
    }

    public virtual void OnThrow(Vector3 throwStartPos, Vector3 throwDirection, float throwForce)
    {
        OnThrowClientRpc(throwStartPos, throwDirection, throwForce);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void OnThrowClientRpc(Vector3 throwStartPos, Vector3 throwDirection, float throwForce)
    {
        //UnparentFromPickupPosition();
        transform.position = throwStartPos;

        EnablePhysics();

        rb.AddForce(throwForce * throwDirection, ForceMode.Impulse);

        thrown = true;
    }

    [Rpc(SendTo.Server)]
    private void BreakServerRpc()
    {
        if(broken)
            return;

        broken = true;

        BreakClientRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void BreakClientRpc()
    {
        Break();
    }

    public virtual void Break()
    {
        onBreakVFX.transform.SetParent(null);
        onBreakVFX.SetActive(true);
        Destroy(onBreakVFX, 5f);

        gameObject.SetActive(false);

        //Destroy(gameObject);
    }

    public void ParentToPickupPosition(Transform pickupPos)
    {
        // transform.SetParent(pickupPos);
        // transform.localPosition = Vector3.zero;
    }

    public void UnparentFromPickupPosition()
    {
        // transform.SetParent(null);
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
            BreakServerRpc();
    }
}
