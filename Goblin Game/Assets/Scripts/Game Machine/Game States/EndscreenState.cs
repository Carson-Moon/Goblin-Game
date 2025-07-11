using UnityEngine;

// Responsible for displaying our end screen and sending our stats to the server.

public class EndscreenState : GameState
{
    [Header("Endscreen Canvas")]
    [SerializeField] CanvasGroup endscreenCanvasGroup;


    public override void StartThisState()
    {
        base.StartThisState();
    }

    public override void EndThisState()
    {
        base.EndThisState();
    }
}
