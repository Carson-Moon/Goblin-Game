using DG.Tweening;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickup : NetworkBehaviour
{
    private Rigidbody _rb;
    private NetworkObject _networkObject;

    [SerializeField] private PickupList _id;
    public PickupList ID => _id;

    [Header("Break")]
    [SerializeField] GameObject breakVFX;
    [SerializeField] AudioSource breakSFX;

    private bool _pickedUp = false;

    private bool _thrown = false;
    public bool Thrown => _thrown;


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _networkObject = GetComponent<NetworkObject>();
    }

    public void OnPickup()
    {
        if(_pickedUp)
        {
            Debug.LogWarning("How are we seeing this object? It's already picked up!");
            return;
        }

        if(!IsOwner)
            _networkObject.RequestOwnership();

        OnPickupClientRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void OnPickupClientRpc()
    {
        ToggleObject(false);
    }

#region Throw
    public void OnThrow(Vector3 startPosition, Vector3 direction, float force)
    {
        OnThrowClientRpc(startPosition, direction, force);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void OnThrowClientRpc(Vector3 startPosition, Vector3 direction, float force)
    {
        transform.position = startPosition;
        ToggleObject(true);

        _rb.AddForce(direction * force, ForceMode.Impulse);

        _thrown = true;
    }

#endregion

#region Break

    void OnCollisionEnter(Collision collision)
    {
        if(_thrown)
            BreakClientRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void BreakClientRpc()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            breakVFX.transform.SetParent(null);
            breakVFX.SetActive(true);

            ToggleObject(false);
        });
        sequence.AppendInterval(5);
        sequence.AppendCallback(() =>
        {
            breakVFX.SetActive(false);
            breakVFX.transform.SetParent(transform);
            breakVFX.transform.localPosition = Vector3.zero;
        });
    }

#endregion

    private void ToggleObject(bool toggle)
    {
        gameObject.SetActive(toggle);
        _rb.isKinematic = !toggle;
    }
}
