using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ServerObjectiveManager : NetworkBehaviour
{
    [SerializeField] ClientObjectiveManager clientObjectiveManager;
    [SerializeField] ObjectiveList objectives;


    void Start()
    {
        Debug.Log(ServerLobbyManager.Instance.ClientIDs.Count);
        foreach(var playerID in ServerLobbyManager.Instance.ClientIDs)
            GiveObjective(playerID);
    }

    public void GiveObjective(ulong playerID)
    {
        var clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams { TargetClientIds = new[] { playerID } }
        };

        int objectiveIndex = objectives.GetRandomObjectiveIndex();

        clientObjectiveManager.ReceiveObjectiveClientRpc(objectiveIndex, clientRpcParams);
    }
}
