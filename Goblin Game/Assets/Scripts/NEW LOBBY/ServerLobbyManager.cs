using System.Collections.Generic;
using System.Data.Common;
using Unity.Netcode;
using UnityEngine;

public class ServerLobbyManager : NetworkBehaviour
{
    public static ServerLobbyManager Instance { get; private set; }

    // Everything we need to store from our players.
    // Client IDs
    [SerializeField] List<ulong> clientIDs = new List<ulong>();
    public List<ulong> ClientIDs => clientIDs;

    // Player Information - Name
    [SerializeField] Dictionary<ulong, PlayerInformation> playerInformations = new Dictionary<ulong, PlayerInformation>();
    public Dictionary<ulong, PlayerInformation> PlayerInformations => playerInformations;

    // GoblinClient - Leads to everything attached to the player prefab
    [SerializeField] Dictionary<ulong, GoblinClient> goblinClients = new Dictionary<ulong, GoblinClient>();
    public Dictionary<ulong, GoblinClient> GoblinClients => goblinClients;

    [SerializeField] ConnectedPlayersDisplay connectedPlayersDisplay;


    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        DontDestroyOnLoad(this);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            NetworkManager.OnClientConnectedCallback += OnPlayerJoinedServer;
            NetworkManager.OnClientDisconnectCallback += OnPlayerLeftServer;
        }
    }

    public void OnPlayerJoinedServer(ulong clientID)
    {
        clientIDs.Add(clientID);

        GoblinClient newGoblinClient = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientID)
                                            .GetComponent<GoblinClient>();
        if (newGoblinClient != null)
        {
            goblinClients.Add(clientID, newGoblinClient);

            var clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams { TargetClientIds = new[] { clientID } }
            };

            newGoblinClient.ServerRequestPlayerInformationClientRpc(clientRpcParams);

            // Client setup will continue after we receive their Player Information. 
        }
        else
        {
            Debug.LogWarning($"Did not find Goblin Client for {clientID}.");
        }
    }

    [Rpc(SendTo.Server)]
    public void ReceiveRequestedPlayerInformationServerRpc(ulong clientID, PlayerInformation pInfo)
    {
        playerInformations.Add(clientID, pInfo);
        Debug.Log($"Received Player Information from {clientID}, their name is {pInfo.Username}");

        // We can continue the new client setup since we have successfully received their Player Information.
        var newClientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams { TargetClientIds = new[] { clientID } }
        };


        foreach (ulong id in clientIDs)
        {
            connectedPlayersDisplay.AddConnectedPlayerDisplayClientRpc(id, playerInformations[id].Username, newClientRpcParams);
        }

        // Existing players need to be updated of this new client.
        var existingClientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams { TargetClientIds = clientIDs.FindAll(x => x != clientID) }
        };

        connectedPlayersDisplay.AddConnectedPlayerDisplayClientRpc(clientID, playerInformations[clientID].Username, existingClientRpcParams);
    }

    public void OnPlayerLeftServer(ulong clientID)
    {
        // Remove from our server list.
        clientIDs.Remove(clientID);
        playerInformations.Remove(clientID);
        goblinClients.Remove(clientID);

        // For all remaining players, update that this client has left.
        var remainingClientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams { TargetClientIds = clientIDs }
        };

        connectedPlayersDisplay.RemoveConnectedPlayerDisplayClientRpc(clientID, remainingClientRpcParams);
    }
}
