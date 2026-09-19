using Unity.Netcode;
using UnityEngine;

public class RaceCondition : ObjectiveCondition
{
    [SerializeField] ObjectiveZone[] orderedRings;
    private int ringIndex = 0;


    public override string GetPanelDisplay()
    {
        return $"Rings: {ringIndex} / {orderedRings.Length}";
    }

#region Setup

    protected override void OnSetupConditionServer()
    {
        // Pass
    }

    [ClientRpc]
    protected override void OnSetupConditionClientRpc()
    {
        ringIndex = 0;
        ProgressRaceRings();
    }

#endregion

    private void ProgressRaceRings()
    {
        orderedRings[ringIndex].EnableZone(OnRingReached);
    }

    private void HideAllRings()
    {
        foreach(var ring in orderedRings)
            HideRing(ring);
    }

    private void HideRing(ObjectiveZone ring)
    {
        ring.DisableZone();
    }

    private void OnRingReached(ObjectiveZone zone)
    {
        HideRing(orderedRings[ringIndex]);

        ringIndex++;
        if(orderedRings.Length == ringIndex)
            Debug.Log("Ring Objective complete!");
        else
            ProgressRaceRings();

        UpdateConditionUI();
    }


}
