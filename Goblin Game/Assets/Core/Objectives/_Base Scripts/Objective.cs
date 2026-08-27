using System;
using Unity.Netcode;
using UnityEngine;

public class Objective : MonoBehaviour
{
    [SerializeField] string objectiveName;
    public string ObjectiveName => objectiveName;

    [SerializeField] string objectiveDescription;
    public string ObjectiveDescription => objectiveDescription;

    [SerializeField] ObjectiveCondition[] conditions;
    public ObjectiveCondition[] Conditions => conditions;

    public event Action<ulong> NotifyServerObjectiveCompleted;


    public void StartObjective(Action<ulong> onComplete)
    {
        NotifyServerObjectiveCompleted += onComplete;

        foreach(var condition in conditions)
            condition.Begin(OnConditionCompleted);
    }

    public void EndObjective()
    {
        NotifyServerObjectiveCompleted = null;

        foreach(var condition in conditions)
            condition.End();
    }

    private void OnConditionCompleted()
    {
        bool allConditionsComplete = true;
        foreach(var condition in conditions)
        {
            if(!condition.IsComplete())
                allConditionsComplete = false;
        }
        
        if(allConditionsComplete)
        {
            if(NetworkManager.Singleton != null)
                ObjectiveCompleted(NetworkManager.Singleton.LocalClientId);
            else
                ObjectiveCompleted(0);
        }
            
    }

    public void ObjectiveCompleted(ulong playerID)
    {
        NotifyServerObjectiveCompleted?.Invoke(playerID);
    }
}
