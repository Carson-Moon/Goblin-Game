using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ServerStartGame : NetworkBehaviour
{
    public static ServerStartGame Instance { get; private set; }

    [SerializeField] string gameScene = "GameScene";
    private Dictionary<ulong, bool> readyToSwitch = new();


    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void AttemptTransitionToGameScene()
    {
        // Make sure we are good to switch.

        StartTransitionToGameSceneServerRpc();
    }

    [Rpc(SendTo.Server)]
    private void StartTransitionToGameSceneServerRpc()
    {
        // Setup our dictionary with all client ids.
        readyToSwitch.Clear();
        foreach (ulong clientID in ServerLobbyManager.Instance.ClientIDs)
        {
            readyToSwitch.Add(clientID, false);
        }

        // Everything we do client side before loading.
        StartTransitionToGameSceneClientRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void StartTransitionToGameSceneClientRpc()
    {
        ulong localClientID = NetworkManager.Singleton.LocalClientId;

        LoadingScreenManager.Instance.EnableLoadingScreen("", "", AlertServerWeAreReadyToSwitch);
    }

    private void AlertServerWeAreReadyToSwitch()
    {
        Debug.Log("We are ready to switch!");
        AlertServerWeAreReadyToSwitchServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [Rpc(SendTo.Server)]
    private void AlertServerWeAreReadyToSwitchServerRpc(ulong clientID)
    {
        readyToSwitch[clientID] = true;

        // Check the dictionary.
        foreach (KeyValuePair<ulong, bool> clientReadiness in readyToSwitch)
        {
            if (clientReadiness.Value == false)
                return;
        }

        LoadGameScene();
    }

    private void LoadGameScene()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(gameScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
