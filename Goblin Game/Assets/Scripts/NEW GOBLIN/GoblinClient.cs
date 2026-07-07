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

    [SerializeField] private NetworkVariable<Color> goblinColor = new NetworkVariable<Color>();
    public Color GoblinColor => goblinColor.Value;

    [Header("Goblin Components")]
    [SerializeField] Camera armOverlayCamera;
    public Camera ArmOverlayCamera => armOverlayCamera;

    [SerializeField] GoblinController goblinController;
    public GoblinController GoblinController => goblinController;

    [SerializeField] GoblinCharacter goblinCharacter;
    public GoblinCharacter GoblinCharacter => goblinCharacter;

    [SerializeField] GoblinMaterialInstantiater goblinMaterials;



    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        goblinName.OnValueChanged += (previousValue, newValue) =>
        {
            Debug.Log($"Goblin Name: {newValue}");
        };

        goblinColor.OnValueChanged += (previousValue, newValue) =>
        {
            Debug.Log($"Goblin Color: {newValue}");
            UpdateColor();
        };

        if(IsLocalPlayer)
        {
            SetUsernameServerRpc(UsernameHolder.Username);

            Color color = ColorHolder.Color;
            SetColorServerRpc(color.r, color.g, color.b);
        }
        else
        {
            UpdateColor();
        }
            
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetUsernameServerRpc(string _username)
    {
        goblinName.Value = _username;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetColorServerRpc(float colorR, float colorG, float colorB)
    {
        goblinColor.Value = new Color(colorR, colorG, colorB);
    }

    [ClientRpc]
    public void ServerRequestPlayerInformationClientRpc(ClientRpcParams clientRpcParams)
    {
        ServerLobbyManager.Instance.ReceiveRequestedPlayerInformationServerRpc
        (
            NetworkManager.Singleton.LocalClientId,
            new PlayerInformation(goblinName.Value)       // THIS WILL CHANGE TO PLAYERPREFS GRAB!
        );
    }

    private void UpdateColor()
    {
        goblinMaterials.Initialize();

        Debug.Log(goblinMaterials.Materials.Count);

        foreach(var mat in goblinMaterials.Materials)
            mat.SetColor("_Hue_Shift", goblinColor.Value);
    }

#region Game Manager Functions
    public void InitialSpawn(Vector3 position, Action onComplete)
    {
        goblinController.Teleport(position);

        onComplete.Invoke();
    }

    public void SetPosition(Vector3 position)
    {
        goblinController.Teleport(position);
    }
#endregion
}
