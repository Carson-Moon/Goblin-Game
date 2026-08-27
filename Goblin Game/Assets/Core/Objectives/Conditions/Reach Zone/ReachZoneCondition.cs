using UnityEngine;

public class ReachZoneCondition : ObjectiveCondition
{
    [SerializeField] ObjectiveZone[] zones;
    [SerializeField] int numberToReach;
    private int reached = 0;


    protected override void OnBegin()
    {
        foreach(var zone in zones)
            zone.EnableZone(OnLocalPlayerEnteredZone);
    }

    protected override void OnEnd()
    {
        foreach(var zone in zones)
            zone.DisableZone();
    }

    public override float GetProgressPercentage()
    {
        return Mathf.Clamp(reached / numberToReach, 0f, 1f);
    }

    public override bool IsComplete()
    {
        return reached >= numberToReach;
    }

    private void OnLocalPlayerEnteredZone()
    {
        reached++;

        if(IsComplete())
            OnConditionCompleted?.Invoke();

        UpdateConditionUI();
    }

    public override string GetPanelDisplay()
    {
        return $"Zones: {reached}/{numberToReach}";
    }
}
