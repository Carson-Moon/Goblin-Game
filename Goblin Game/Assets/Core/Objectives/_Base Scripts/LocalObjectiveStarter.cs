using System.Collections;
using UnityEngine;

public class LocalObjectiveStarter : MonoBehaviour
{
    [SerializeField] Objective objective;
    [SerializeField] float initialStartWait;


    void Start()
    {
        StartCoroutine(StartObjectiveWithWait());
    }

    IEnumerator StartObjectiveWithWait()
    {
        yield return new WaitForSeconds(initialStartWait);

        if(objective != null)
        {
            Debug.Log("Objective started!");
            objective.StartObjective(OnObjectiveCompleteHandler);
            ObjectiveCanvas.Instance.Initialize(objective);
        }
        else
            Debug.Log("Erm... We don't have an objective to start locally.");
    }

    private void OnObjectiveCompleteHandler(ulong playerID)
    {
        Debug.Log("Objective was completed.");
    }
}
