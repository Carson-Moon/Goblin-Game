using UnityEngine;

// Goblin right hand actions:
// Grab if hand is empty.
// Vacuum if hand is not empty.

public class GoblinRightClick : AbstractButtonInputContainer
{
    [SerializeField] GrabAction grabAction;
    [SerializeField] VacuumAction vacuumAction;
    

    protected override void OnPressedAction()
    {
        if (grabAction.CurrentPickup == null)
        {
            grabAction.AttemptPickup();
        }
        else
        {
            // Start vacuuming if we have a jar!
            if (!vacuumAction.IsVacuuming)
                vacuumAction.AttemptStartVacuum();
        }

    }

    protected override void OnReleasedAction()
    {
        if(vacuumAction.IsVacuuming)
            vacuumAction.AttemptStopVacuum();
    }
}
