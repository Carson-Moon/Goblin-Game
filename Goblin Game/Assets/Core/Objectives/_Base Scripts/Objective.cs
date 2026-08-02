using System;
using UnityEngine;

public abstract class Objective : MonoBehaviour
{
    [SerializeField] string objectiveName;
    public string ObjectiveName => objectiveName;

    [SerializeField] string objectiveDescription;
    public string ObjectiveDescription => objectiveDescription;

    public int Variations => listener.Variations;

    [SerializeField] ObjectiveListener listener;

    public event Action<ulong> NotifyServerObjectiveCompleted;


    public void StartObjective(int variation)
    {
        listener.SetupObjective(variation);
        listener.SignalObjectiveCompleted += ObjectiveCompleted;
    }

    public void EndObjective()
    {
        listener.CleanUpObjective();
        listener.SignalObjectiveCompleted -= ObjectiveCompleted;
    }

    public void ObjectiveCompleted(ulong playerID)
    {
        NotifyServerObjectiveCompleted?.Invoke(playerID);
    }
}
