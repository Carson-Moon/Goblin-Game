using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

// This is the in and out of a goblin. Things will look here for information!

public class GoblinClient : NetworkBehaviour
{
    [Header("Goblin Client Information")]
    private NetworkVariable<FixedString32Bytes> goblinName = new NetworkVariable<FixedString32Bytes>("Mind Goblin");
    public FixedString32Bytes GoblinName => goblinName.Value;

    [Header("Goblin Components")]
    [SerializeField] Camera armOverlayCamera;
    public Camera ArmOverlayCamera => armOverlayCamera;

    [SerializeField] Goblin goblin;
    public Goblin Goblin => goblin;

    [SerializeField] GoblinCharacter goblinCharacter;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Set our name here from player prefs.
    }

    [ClientRpc]
    public void ServerRequestPlayerInformationClientRpc(ClientRpcParams clientRpcParams)
    {
        ServerLobbyManager.Instance.ReceiveRequestedPlayerInformationServerRpc
        (
            NetworkManager.Singleton.LocalClientId,
            new PlayerInformation(GoblinName)       // THIS WILL CHANGE TO PLAYERPREFS GRAB!
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
