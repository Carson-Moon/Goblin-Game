using UnityEngine;

// Responsible for displaying our end screen and sending our stats to the server.

public class EndscreenState : GameState
{
    [Header("Endscreen Canvas")]
    [SerializeField] Abstract_Canvas_Animation endscreenAnimation;

    [Header("Continue Timer")]
    [SerializeField] float continueTimerLength;
    [SerializeField] float currentContinueTimer;


    // Setup our endscreen canvas.
    public void SetupEndscreen()
    {

    }

    // Enable our endscreen.
    public void EnableEndscreen()
    {
        endscreenAnimation.PlayAnimation();
    }

    public override void StartThisState()
    {
        base.StartThisState();

        SetupEndscreen();
        EnableEndscreen();
    }

    public override void EndThisState()
    {
        base.EndThisState();
    }
}
