using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InitialSetupState : GameState
{
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();


    void Start()
    {
        if (IsServer)
            OnStartStateServerRpc();
    }

    protected override void OnStartStateServer()
    {
        base.OnStartStateServer();

        // Create a lock for this state.
        CreateLock();

        // Send each player their spawn point.
        spawnPoints.Shuffle();
        int spIndex = 0;
        foreach (ulong clientID in ServerLobbyManager.Instance.ClientIDs)
        {
            var clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams { TargetClientIds = new[] { clientID } }
            };

            MoveToSpawnPointClientRpc(spawnPoints[spIndex].position, clientRpcParams);
            spIndex++;
        }
    }

    [ClientRpc]
    private void MoveToSpawnPointClientRpc(Vector3 spawnPosition, ClientRpcParams clientRpcParams)
    {
        GoblinClientPointer.LocalGoblinClient().ToggleMovement(false);

        Debug.Log($"We are spawning at this position: {spawnPosition}!");

        OnClientUnlocked = null;
        OnClientUnlocked += ClientUnlock;

        GoblinClientPointer.LocalGoblinClient().InitialSpawn(spawnPosition, OnClientUnlocked);
    }
}
