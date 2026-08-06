using System;
using UnityEngine;

public abstract class ObjectiveCondition : MonoBehaviour
{
    protected Action OnConditionCompleted;


    public void Begin(Action onComplete)
    {
        OnConditionCompleted += onComplete;
        OnBegin();
    }

    public void End()
    {
        OnConditionCompleted = null;
        OnEnd();
    }


    protected abstract void OnBegin();
    protected abstract void OnEnd();
    public abstract bool IsComplete();
    public abstract float GetProgressPercentage();
}
