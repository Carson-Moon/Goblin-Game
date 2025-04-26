using UnityEngine;
using Unity.Netcode;

public class Network_Goblin : NetworkBehaviour
{
    [Header("Goblin Logic")]
    [SerializeField] Transform goblinOrientation;
    [SerializeField] MeshRenderer bodyRen;
    [SerializeField] MeshRenderer noseRen;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        goblinOrientation = Goblin.instance.GetOrientation();

        if(IsOwner)
        {
            bodyRen.enabled = false;
            noseRen.enabled = false;
        }
    }

    void Update()
    {
        transform.position = goblinOrientation.position;
        transform.rotation = goblinOrientation.rotation;
    }
}
