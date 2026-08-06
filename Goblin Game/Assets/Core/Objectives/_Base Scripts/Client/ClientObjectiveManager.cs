using Unity.Netcode;
using UnityEngine;

public class ClientObjectiveManager : NetworkBehaviour
{
    [SerializeField] ObjectiveList objectives;


    [ClientRpc]
    public void ReceiveObjectiveClientRpc(int objectiveIndex, ClientRpcParams clientRpcParams = default)
    {
        Objective objective = Instantiate(objectives.GetObjectiveByIndex(objectiveIndex));
        objective.StartObjective(NotifyServerObjectiveCompleteServerRpc);
        Debug.Log($"Received objective {objectiveIndex}.");
    }

    [ServerRpc(RequireOwnership = false)]
    private void NotifyServerObjectiveCompleteServerRpc(ulong playerID)
    {
        Debug.Log($"{playerID.GetUsername()} just completed their objective!");
    }
}
