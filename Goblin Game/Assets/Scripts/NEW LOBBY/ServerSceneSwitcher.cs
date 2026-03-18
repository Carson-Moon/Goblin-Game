using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ServerSceneSwitcher : NetworkBehaviour
{
    public static ServerSceneSwitcher Instance { get; private set; }

    [SerializeField] string gameScene = "GameScene";
    [SerializeField] string awardsScene = "AwardsCeremony";
    private string targetScene;

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
        targetScene = gameScene;
        StartTransitionToSceneServerRpc();
    }

    public void AttemptTransitionToAwardsScene()
    {
        targetScene = awardsScene;
        StartTransitionToSceneServerRpc();
    }

    [Rpc(SendTo.Server)]
    private void StartTransitionToSceneServerRpc()
    {
        // Setup our dictionary with all client ids.
        readyToSwitch.Clear();
        foreach (ulong clientID in ServerLobbyManager.Instance.ClientIDs)
        {
            readyToSwitch.Add(clientID, false);
        }

        // Everything we do client side before loading.
        StartTransitionToSceneClientRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void StartTransitionToSceneClientRpc()
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
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
