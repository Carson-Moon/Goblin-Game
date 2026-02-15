using System;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Vivox;
using UnityEngine;

// This is the in and out of a goblin. Things will look here for information!

public class GoblinClient : NetworkBehaviour
{
    [Header("Goblin Client Information")]
    [SerializeField] private NetworkVariable<FixedString32Bytes> goblinName = new NetworkVariable<FixedString32Bytes>("Mind Goblin");
    public FixedString32Bytes GoblinName => goblinName.Value;

    [Header("Goblin Components")]
    [SerializeField] Camera armOverlayCamera;
    public Camera ArmOverlayCamera => armOverlayCamera;

    [SerializeField] Goblin goblin;
    public Goblin Goblin => goblin;

    [SerializeField] GoblinCharacter goblinCharacter;
    public GoblinCharacter GoblinCharacter => goblinCharacter;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        goblinName.OnValueChanged += (previousValue, newValue) =>
        {
            Debug.Log($"Goblin Name: {newValue}");
        };

        // Set our name here from player prefs.
        if(IsLocalPlayer)
            SetUsernameServerRpc(UsernameHolder.Username);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetUsernameServerRpc(string _username)
    {
        goblinName.Value = _username;
    }

    [ClientRpc]
    public void ServerRequestPlayerInformationClientRpc(ClientRpcParams clientRpcParams)
    {
        ServerLobbyManager.Instance.ReceiveRequestedPlayerInformationServerRpc
        (
            NetworkManager.Singleton.LocalClientId,
            new PlayerInformation(PlayerPrefs.GetString(UsernameHolder.Username, StaticPlayerPrefsHelper.DefaultUsername))       // THIS WILL CHANGE TO PLAYERPREFS GRAB!
        );
    }

    #region Game Manager Functions
    public void InitialSpawn(Vector3 position, Action onComplete)
    {
        goblin.Teleport(position);

        onComplete.Invoke();
        //LoadingScreenManager.Instance.DisableLoadingScreen(onComplete);
    }

    public void ToggleMovement(bool toggle)
    {
        goblinCharacter.ToggleMovement(toggle);
    }
    #endregion
}
