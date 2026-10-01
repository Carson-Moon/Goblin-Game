using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class NetworkObjectiveStarter : NetworkBehaviour
{
    [SerializeField] LevelObjectives levelObjectives;
    [SerializeField] float initialStartWait;
    [SerializeField] float betweenObjectivesWait;
    [SerializeField] Timer timer;
    [SerializeField] int pointsToWin = 3;

    Dictionary<ulong, int> playerObjectivePoints = new();

    private Objective currentObjective;



    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            if(IsServer)
                PreObjectiveClientRpc();
        }
    }

    void Start()
    {
        StartCoroutine(StartObjectives());
    }

    IEnumerator StartObjectives()
    {
        yield return new WaitForSeconds(5);

        if(IsServer)
            PreObjectiveClientRpc();
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
        GetObjectiveWinnersClientRpc(winners.ToArray());

        if(currentObjective != null)
            currentObjective.EndObjectiveServer();

        Debug.Log("Objective is over.");

        PreObjectiveClientRpc();
    }

    [ClientRpc]
    private void GetObjectiveWinnersClientRpc(ulong[] winners)
    {
        foreach(var winner in winners)
        {
            if(playerObjectivePoints.ContainsKey(winner))
                playerObjectivePoints[winner]++;
            else
                playerObjectivePoints.Add(winner, 1);
        }

        ObjectiveWinnerDisplay.Instance.DisplayWinners(winners.ToList(), playerObjectivePoints);

        if(IsServer)
        {
            var orderedPlayers = playerObjectivePoints.OrderByDescending(x => x.Value);
            if(orderedPlayers.First().Value == pointsToWin)
            {
                var overallWinners = playerObjectivePoints.Where(x => x.Value == pointsToWin).Select(x => x.Key).ToArray();
                GetOverallObjectiveWinnersClientRpc(overallWinners);
            }
        }
    }

    [ClientRpc]
    private void GetOverallObjectiveWinnersClientRpc(ulong[] winners)
    {
        Debug.Log("OVerall winners!");
    }
}
