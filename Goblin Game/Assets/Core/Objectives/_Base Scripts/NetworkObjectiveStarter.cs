using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class NetworkObjectiveStarter : NetworkBehaviour
{
    [SerializeField] LevelObjectives levelObjectives;
    [SerializeField] float initialStartWait;
    [SerializeField] float betweenObjectivesWait;
    [SerializeField] Timer timer;

    private Objective currentObjective;



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
            currentObjective = levelObjectives.GetObjectiveByIndex(objectiveIndex);
            if(currentObjective != null)
            {
                currentObjective.StartObjectiveServer(StopObjectiveServer);
                ObjectiveCanvas.Instance.Initialize(currentObjective);
            }
            else
            {
                Debug.LogWarning("Objective was null.");
            }
        }
    }

    private void StopObjectiveServer(List<ulong> winners)
    {
        DisplayWinScreenClientRpc(winners.ToArray());

        if(currentObjective != null)
            currentObjective.EndObjectiveServer();

        Debug.Log("Objective is over.");

        PreObjectiveClientRpc();
    }

    [ClientRpc]
    private void DisplayWinScreenClientRpc(ulong[] winners)
    {
        ObjectiveWinnerDisplay.Instance.DisplayWinners(winners.ToList());
    }
}
