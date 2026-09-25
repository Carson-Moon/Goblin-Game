using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalObjectiveStarter : MonoBehaviour
{
    [SerializeField] LevelObjectives levelObjectives;
    [SerializeField] float initialStartWait;
    [SerializeField] float betweenObjectivesWait;
    [SerializeField] Timer timer;

    [Header("UI")]
    [SerializeField] ObjectiveWinnerDisplay winnerUI;


    void Start()
    {
        StartObjective(initialStartWait);
    }

    private void StartObjective(float wait)
    {
        StartCoroutine(StartObjectiveWithWait(wait));
    }

    IEnumerator StartObjectiveWithWait(float wait)
    {
        yield return new WaitForSeconds(wait);

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
                objective.StartObjectiveServer(OnObjectiveCompleteHandler);
                ObjectiveCanvas.Instance.Initialize(objective);
            }
            else
            {
                Debug.LogWarning("Objective was null.");
            }
        }
    }

    private void OnObjectiveCompleteHandler(List<ulong> playerIDs)
    {
        Debug.Log("Objective was completed.");
        ObjectiveCanvas.Instance.ResetUI();
        winnerUI.DisplayWinner(playerIDs.First());

        timer.StartTimer(betweenObjectivesWait, () => StartObjective(0));
    }
}
