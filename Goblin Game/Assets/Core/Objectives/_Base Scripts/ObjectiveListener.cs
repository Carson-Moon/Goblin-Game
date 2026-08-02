using System;
using UnityEngine;

public abstract class ObjectiveListener : MonoBehaviour
{
    public abstract int Variations { get; }

    public event Action<ulong> SignalObjectiveCompleted;

    public abstract void SetupObjective(int variation);
    public abstract void CleanUpObjective();
    public abstract void OnLocalObjectiveCompleted();
    protected void ObjectiveCompleted(ulong playerID)
    {
        SignalObjectiveCompleted?.Invoke(playerID);
        OnLocalObjectiveCompleted();
    }
}
