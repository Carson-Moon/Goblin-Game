using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class LobbyManager : NetworkBehaviour
{
    

    // Singleton
    public static LobbyManager instance {get; private set;}

    [Header("Player Count")]
    [SerializeField] private NetworkVariable<int> playerCount = new(1);
    [SerializeField] private TextMeshProUGUI playerCountText;

    [Header("Player Information")]
    [SerializeField] private NetworkList<NetworkBehaviourReference> playerList = new NetworkList<NetworkBehaviourReference>(null, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private List<Client_Goblin> players = new List<Client_Goblin>();
    [SerializeField] private TextMeshProUGUI playerNamesText;
    [SerializeField] bool allPlayersReady = false;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Update()
    {
        playerCountText.text = playerCount.Value.ToString() + " PLAYERS";
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if(IsHost)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectedCallback;
        }
    }

    private void OnClientConnectedCallback(ulong obj)
    {
        // Update our player count.
        playerCount.Value++;

        UpdatePlayerCountTextRPC();
    }

    private void OnClientDisconnectedCallback(ulong obj)
    {
        playerCount.Value--;

        UpdatePlayerCountTextRPC();
    }

    public void UpdatePlayerCountText()
    {
        playerCountText.text = playerCount.Value.ToString() + " PLAYERS";
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void UpdatePlayerCountTextRPC()
    {
        playerCountText.text = playerCount.Value.ToString() + " PLAYERS";
    }

    [Rpc(SendTo.Server)]
    public void AddClientToListRPC(NetworkBehaviourReference networkRef)
    {
        playerList.Add(networkRef);
        //networkRef.TryGet(out NetworkBehaviour nBehaviour, NetworkManager.Singleton);
        //players.Add(nBehaviour.GetComponent<Client_Goblin>());

        StartCoroutine(ListCooldown());

        TestPrintRPC();
    }

    IEnumerator ListCooldown()
    {
        yield return new WaitForSeconds(.05f);

        BuildPlayerNameListRPC();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void BuildPlayerNameListRPC()
    {
        // Build our player list.
        players.Clear();
        for (int i = 0; i < playerList.Count; i++)
        {
            // Get network behaviour this reference is referencing
            playerList[i].TryGet(out NetworkBehaviour nBehaviour, NetworkManager.Singleton);

            if (nBehaviour != null)
            {
                players.Add(nBehaviour.GetComponent<Client_Goblin>());
                print("JUST ADDED PLAYER " + i + ": " + nBehaviour.GetComponent<Client_Goblin>().GetName());
            }
            else
            {
                print("PLAYER " + i + ": " + "COULD NOT FIND CLIENT GOBLIN SCRIPT.");
            }
        }

        // Build our list of names.
        int count = players.Count;
        string playerNames = "";
        for (int i = 0; i < count; i++)
        {
            playerNames += players[i].GetName() + "\n";
        }

        playerNamesText.text = playerNames;
    }

    private void DeterminePlayerReadiness()
    {
        int count = players.Count;
        for(int i=0; i<count; i++)
        {
            
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void TestPrintRPC()
    {
        for(int i=0; i<playerList.Count; i++)
        {
            // Get network behaviour this reference is referencing
            playerList[i].TryGet(out NetworkBehaviour nBehaviour, NetworkManager.Singleton);

            if(nBehaviour != null)
            {
                print("PLAYER " + i + ": " + nBehaviour.GetComponent<Client_Goblin>().GetName());
            }
            else
            {
                print("PLAYER " + i + ": " + "COULD NOT FIND CLIENT GOBLIN SCRIPT.");
            }
        }
    }
}
