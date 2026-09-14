using UnityEngine;

public class ReachZoneCondition : ObjectiveCondition
{
    [SerializeField] ObjectiveZone[] zones;
    private int numberToReach => zones.Length;
    private int reached = 0;


    protected override void OnBegin()
    {
        reached = 0;

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
