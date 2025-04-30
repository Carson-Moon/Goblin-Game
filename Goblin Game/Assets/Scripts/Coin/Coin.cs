using Unity.Netcode;
using UnityEngine;

public class Coin : NetworkBehaviour
{
    // Runtime
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float spawnForce;
    [SerializeField] private float spawnTorque;

    [Header("Collect Cooldown")]
    [SerializeField] bool canCollect = false;
    [SerializeField] float collectCooldown;


    void Update()
    {
        collectCooldown -= Time.deltaTime;
        collectCooldown = Mathf.Clamp(collectCooldown, 0, 10);

        if(!canCollect && collectCooldown == 0)
        {
            canCollect = true;
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Apply random force and torque.
        Vector3 randomForce = Vector3.up + (Vector3.right * Random.Range(-1f, 1f)) + (Vector3.forward * Random.Range(-1f, 1f));

        rb.AddForce(spawnForce * randomForce, ForceMode.Impulse);
        rb.AddTorque(spawnTorque * Vector3.right, ForceMode.Impulse);
    }

    public void OnCollect()
    {
        DespawnCoinRPC();
    }

    [Rpc(SendTo.Server)]
    private void DespawnCoinRPC()
    {
        GetComponent<NetworkObject>().Despawn();
    }

    public bool CanCollect()
    {
        return canCollect;
    }
}
