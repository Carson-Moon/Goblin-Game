using System.Collections;
using UnityEngine;

public class LocalObjectiveStarter : MonoBehaviour
{
    [SerializeField] LevelObjectives levelObjectives;
    [SerializeField] float initialStartWait;


    void Start()
    {
        StartCoroutine(StartObjectiveWithWait());
    }

    IEnumerator StartObjectiveWithWait()
    {
        yield return new WaitForSeconds(initialStartWait);

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
                objective.StartObjective(OnObjectiveCompleteHandler);
                ObjectiveCanvas.Instance.Initialize(objective);
            }
            else
            {
                Debug.LogWarning("Objective was null.");
            }
        }
    }

    private void OnObjectiveCompleteHandler(ulong playerID)
    {
        Debug.Log("Objective was completed.");
    }
}
