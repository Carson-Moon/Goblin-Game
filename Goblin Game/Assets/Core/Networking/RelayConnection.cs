using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using System;

public class RelayConnection : MonoBehaviour
{
    public static RelayConnection Instance {get; private set;}

    [SerializeField] private string joinCode = string.Empty;
    public string JoinCode => joinCode;

    public event Action onClientStarted;


    void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    void Start()
    {
        BindConnectionActions(true);
    }

    private void BindConnectionActions(bool bind)
    {
        if(bind)
        {
            NetworkManager.Singleton.OnClientStarted += OnClientStarted;
        }
        else
        {
            if(NetworkManager.Singleton != null)
                NetworkManager.Singleton.OnClientStarted -= OnClientStarted;
        }
    }

    private void OnClientStarted()
    {
        onClientStarted?.Invoke();
    }

    #region Starting a Lobby
    public async void StartHosting(Action onSuccess)
    {
        try
        {
            Debug.Log("Starting to host a lobby...");
            await StartRelayAsHost();
            Debug.Log($"Successfully hosted a lobby. Join code is <color=green>{joinCode}</color>");
            onSuccess?.Invoke();
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    public async Task StartRelayAsHost()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(8);

            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            RelayServerData relayServerData = AllocationUtils.ToRelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            StartLobbyAsHost();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    private void StartLobbyAsHost()
    {
        if(NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
        {
            Debug.LogWarning("We are already in a lobby. Cannot start another.");
            return;
        }

        NetworkManager.Singleton.StartHost();
    }
#endregion

#region Joining a Lobby
    public async void StartJoining(string _joinCode, Action onSuccess)
    {
        try
        {
            Debug.Log("Starting to join a lobby...");
            await JoinRelayAsClient(_joinCode);
            Debug.Log($"Successfully joined a lobby. Join code is <color=green>{joinCode}</color>");
            onSuccess?.Invoke();
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    public async Task JoinRelayAsClient(string _joinCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = AllocationUtils.ToRelayServerData(joinAllocation, "dtls");
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            joinCode = _joinCode;

            JoinLobbyAsClient();
        }
        catch(RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    private void JoinLobbyAsClient()
    {
        if(NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
        {
            Debug.LogWarning("We are already in a lobby. Cannot join another.");
            return;
        }

        NetworkManager.Singleton.StartClient();
    }
#endregion

    void OnDestroy()
    {
        BindConnectionActions(false);
    }
}
