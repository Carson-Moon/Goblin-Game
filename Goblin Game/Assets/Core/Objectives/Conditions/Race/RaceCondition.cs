using UnityEngine;

public class RaceCondition : ObjectiveCondition
{
    [SerializeField] ObjectiveZone[] orderedRings;
    private int ringIndex = 0;


    protected override void OnBegin()
    {
        ringIndex = 0;
        orderedRings[ringIndex].EnableZone(PlayerReachedRing);
    }

    public override bool IsComplete()
    {
        return orderedRings.Length == ringIndex;
    }

    protected override void OnEnd()
    {
        foreach(var zone in orderedRings)
            zone.DisableZone();
    }

    public override string GetPanelDisplay()
    {
        return $"Rings: {ringIndex} / {orderedRings.Length}";
    }

    public override float GetProgressPercentage()
    {
        return (float) ringIndex / orderedRings.Length;
    }

    
    private void PlayerReachedRing()
    {
        ringIndex++;

        if(!IsComplete())
            orderedRings[ringIndex].EnableZone(PlayerReachedRing);
        else
            OnConditionCompleted?.Invoke();
            
        UpdateConditionUI();
    }
}
