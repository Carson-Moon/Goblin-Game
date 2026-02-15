using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Vivox;

//using Unity.Services.Vivox;
using UnityEngine;
using UnityEngine.Events;

public class TestRelay : MonoBehaviour
{
//     [Header("Debug Buttons")]
//     public bool createRelay = false;
//     public bool joinRelay = false;

//     [Header("Relay Settings")]
//     public string joinCode;
//     public TextMeshProUGUI joinCodeText;
//     //public TextMeshPro inLobbyText;
//     public TMP_InputField codeInput;

//     [Header("On Lobby Started/Joined")]
//     public UnityEvent onLobbyJoined;

//     //private VivoxParticipant vivoxParticipant = null;


//     private void Update()
//     {
//         if (createRelay)
//         {
//             CreateRelay();
//             createRelay = false;
//         }

//         if (joinRelay)
//         {
//             JoinRelay(joinCode);
//             joinRelay = false;
//         }
//     }

//     // Host creates relay...
//     public async void CreateRelay()
//     {
//         // The number is maximum number of conenctions, not including the host!
//         try
//         {
//             Allocation allocation = await RelayService.Instance.CreateAllocationAsync(9);

//             joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
//             if (joinCode != null) joinCodeText.text = joinCode;

//             RelayServerData relayServerData = AllocationUtils.ToRelayServerData(allocation, "dtls");

//             NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

//             StartLobby();
//         }
//         catch (RelayServiceException e)
//         {
//             Debug.Log(e);
//         }
//     }

//     // Client joins relay...
//     public async void JoinRelay(string joinCode){
//         try
//         {
//             Debug.Log("Joining Relay with " + joinCode);
//             JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

//             RelayServerData relayServerData = AllocationUtils.ToRelayServerData(joinAllocation, "dtls");
            
//             NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

//             JoinLobby();
//         }
//         catch(RelayServiceException e)
//         {
//             Debug.Log(e);
//         }
//     }

//     // Client joins relay...
//     public async void JoinRelay(){
//         try
//         {
//             string roomCode = codeInput.text;
//             joinCode = codeInput.text.ToUpper();

//             Debug.Log("Joining Relay with " + roomCode);
//             JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(roomCode);

//             RelayServerData relayServerData = AllocationUtils.ToRelayServerData(joinAllocation, "dtls");

//             NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

//             JoinLobby();
//         }
//         catch(RelayServiceException e)
//         {
//             Debug.Log(e);
//         }
//     }

// #region Joining and Leaving Lobbies
//     // Start a lobby!
//     public void StartLobby(){
//         // Ensure we are not already in a lobby...
//         if(NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
//         {
//             print("Already in a lobby!");
//             return;
//         }
            
//         NetworkManager.Singleton.StartHost();

//         OnStartLobby();
//     }

//     // Changes to make when a lobby is successfully started/joined.
//     private void OnStartLobby()
//     {
//         onLobbyJoined.Invoke();

//         //joinCodeText.text = "Current Join Code: " + joinCode;
//         joinCodeText.text = joinCode;
//     }

//     // Join a lobby.
//     public void JoinLobby(){
//         // Ensure we are not already in a lobby...
//         if(NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
//         {
//             print("Already in a lobby!");
//             return;
//         }
            
//         NetworkManager.Singleton.StartClient();

//         OnStartLobby();
//     }

//     // Leave a lobby.
//     public void LeaveLobby(){
//         if(NetworkManager.Singleton.IsHost)
//         {
//             print("Host is leaving the lobby...");
//         }
//         else if(NetworkManager.Singleton.IsClient)
//         {
//             print("Client is leaving the lobby...");
//         }

//         NetworkManager.Singleton.Shutdown();
//     }

// #endregion
}
