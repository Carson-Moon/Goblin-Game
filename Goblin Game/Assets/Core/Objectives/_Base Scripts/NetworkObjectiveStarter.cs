using Unity.Netcode;
using UnityEngine;

public class NetworkObjectiveStarter : NetworkBehaviour
{
    [SerializeField] LevelObjectives levelObjectives;
    [SerializeField] float initialStartWait;
    [SerializeField] float betweenObjectivesWait;
    [SerializeField] Timer timer;



    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            if(IsServer)
                PreObjectiveClientRpc();
        }
    }


    [ClientRpc]
    private void PreObjectiveClientRpc()
    {
        timer.StartTimer(betweenObjectivesWait, IsServer ? StartObjectiveServer : null);
    }

    private void StartObjectiveServer()
    {
        Vector2Int objectiveIndex = levelObjectives.GetRandomObjectiveIndex();
        if(objectiveIndex.x == -1)
        {
            Debug.LogWarning("Did not find any objectives!");
        }
        else
        {
            Debug.Log("Objective started!");
            Objective objective = levelObjectives.GetObjectiveByIndex(objectiveIndex);
            if(objective != null)
            {
                objective.StartObjectiveServer();
                ObjectiveCanvas.Instance.Initialize(objective);
            }
            else
            {
                Debug.LogWarning("Objective was null.");
            }
        }
    }
}
