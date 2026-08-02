using Unity.Netcode;
using UnityEngine;

public class ClientObjectiveManager : NetworkBehaviour
{
    [SerializeField] ObjectiveList objectives;


    [ClientRpc]
    public void ReceiveObjectiveClientRpc(int objectiveIndex, int variation, ClientRpcParams clientRpcParams = default)
    {
        Objective objective = Instantiate(objectives.GetObjectiveByIndex(objectiveIndex));
        objective.StartObjective(variation);
        Debug.Log($"Received objective {objectiveIndex} with variation {variation}.");
    }
}
