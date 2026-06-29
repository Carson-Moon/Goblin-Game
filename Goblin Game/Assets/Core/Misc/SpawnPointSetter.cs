using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class SpawnPointSetter : NetworkBehaviour
{
    [SerializeField] List<Transform> spawnPoints = new();


    public void MovePlayersToSpawnPoints()
    {
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log($"We are spawning at this position: {spawnPosition}!");

        GoblinClientPointer.LocalGoblinClient().SetPosition(spawnPosition);

        GoblinClientPointer.LocalGoblinClient().GoblinController.RemoveAllLocks();
        LoadingScreenManager.Instance.DisableLoadingScreen();
    }
}
