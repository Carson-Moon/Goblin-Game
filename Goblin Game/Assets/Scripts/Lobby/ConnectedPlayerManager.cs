using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public ulong clientID;
    public string playerName;

    public PlayerData(ulong id, string name)
    {
        clientID = id;
        playerName = name;
    }
}

public class ConnectedPlayerManager : NetworkBehaviour
{
    // Singleton
    public static ConnectedPlayerManager instance { get; private set; }

    [Header("Connected Players List")]
    [SerializeField] private List<ulong> connectedIDs = new List<ulong>();

    [Header("Player Data Dictionary")]
    [SerializeField] private Dictionary<ulong, PlayerData> playerDatas = new Dictionary<ulong, PlayerData>();

    [Header("Client Goblin Dictionary")]
    [SerializeField] private Dictionary<ulong, Client_Goblin> clientGoblins = new Dictionary<ulong, Client_Goblin>();

    [Header("Client State Machine Dictionary")]
    [SerializeField] private Dictionary<ulong, ClientMachineInteractor> clientMachines = new Dictionary<ulong, ClientMachineInteractor>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            NetworkManager.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }

#region Connections
    private void OnClientConnected(ulong clientID)
    {
        print($"Client Connected: {clientID}");

        connectedIDs.Add(clientID);

        // Setup player data.
        StartCoroutine(SetupPlayerData(clientID));
    }

    IEnumerator SetupPlayerData(ulong clientID)
    {
        // Ensure the player is spawned.
        yield return new WaitForSeconds(0.3f);

        // Find the network object attatched to this client id.
        NetworkObject clientNetworkObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientID);

        // If we found the object, grab the client goblin script and retrieve data.
        if (clientNetworkObject != null)
        {
            Client_Goblin clientGoblin = clientNetworkObject.GetComponentInChildren<Client_Goblin>();

            // If we found the script, grab the information and add to our dictionary.
            if (clientGoblin != null)
            {
                AddClientGoblinToDictionary(clientID, clientGoblin);

                PlayerData newData = new PlayerData(clientID, clientGoblin.GetName());
                AddPlayerDataToDictionary(clientID, newData);

                AddClientMachineInteractorToDictionary(clientID, clientGoblin.GetComponent<ClientMachineInteractor>());
            }

            // If we did not find the script, display error.
            else
            {
                Debug.LogError($"Could not find {clientID} Client_Goblin script! Mwahahahahah!");
            }
        }

        // If we did not find the object, display error.
        else
        {
            Debug.LogError($"Could not find {clientID} object! Mwahahahahah!");
        }
    }

    private void AddPlayerDataToDictionary(ulong clientID, PlayerData playerData)
    {
        playerDatas.Add(clientID, playerData);

        // Test to make sure data was saved.
        if (playerDatas.TryGetValue(clientID, out PlayerData retrievedData))
        {
            print($"Newly Saved Player Data: {retrievedData.clientID}, {retrievedData.playerName}!");
        }
    }

    private void AddClientGoblinToDictionary(ulong clientID, Client_Goblin clientGoblin)
    {
        clientGoblins.Add(clientID, clientGoblin);
    }

    private void AddClientMachineInteractorToDictionary(ulong clientID, ClientMachineInteractor clientInteractor)
    {
        clientMachines.Add(clientID, clientInteractor);

        // Test to make sure data was saved.
        if (clientMachines.TryGetValue(clientID, out ClientMachineInteractor retrievedData))
        {
            print("Saved Client Interactor!");
        }
    }
    #endregion

    public Client_Goblin GetClientGoblin(ulong clientID)
    {
        if (clientGoblins.TryGetValue(clientID, out Client_Goblin clientGoblin))
        {
            return clientGoblin;
        }
        else
        {
            return null;
        }
    }

    private void OnClientDisconnected(ulong clientID)
    {
        print($"Client Disconnected: {clientID}");

        connectedIDs.Remove(clientID);
    }

    public List<ulong> GetClientIDs()
    {
        return connectedIDs;
    }

    public Dictionary<ulong, ClientMachineInteractor> GetClientInteractors()
    {
        return clientMachines;
    }
}
