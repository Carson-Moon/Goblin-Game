using System.Collections.Generic;
using UnityEngine;

public class RaceListener : ObjectiveListener
{
    [SerializeField] RaceDestination[] objectives;
    public override int Variations => objectives.Length;

    private RaceDestination currentObjective;


    public override void SetupObjective(int variation)
    {
        currentObjective = objectives[variation];
        currentObjective.Initialize(ObjectiveCompleted);
    }

    public override void CleanUpObjective()
    {
        
    }

    public override void OnLocalObjectiveCompleted()
    {
        Debug.Log("This is when YOU complete the objective.");
    }

    private void TrackDestinationDistance()
    {
        
    }
}
