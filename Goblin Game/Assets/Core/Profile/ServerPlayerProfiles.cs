using Unity.Netcode;
using UnityEngine;

public class ServerPlayerProfiles : NetworkBehaviour
{
    [SerializeField] NetworkList<PlayerProfile> playerProfiles;


    void Awake()
    {
        playerProfiles = new NetworkList<PlayerProfile>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
    }

    private void OnPlayerProfilesListChanged(NetworkListEvent<PlayerProfile> changeEvent)
    {
        Debug.Log($"List Changed! Event Type: {changeEvent.Type} at Index: {changeEvent.Index}");
    }
}
