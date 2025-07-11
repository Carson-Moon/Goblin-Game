using TMPro;
using UnityEngine;

// Responsible for tracking our match time. Stops the players movement when time is up!

public class TimedGameplayState : GameState
{
    [Header("Canvas Settings")]
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject stopText;

    [Header("Match Settings")]
    [SerializeField] bool isTimerActive;
    [SerializeField] float matchTime;
    [SerializeField] float currentMatchTimer;


    void Update()
    {
        if (isTimerActive)
        {
            currentMatchTimer -= Time.deltaTime;
            DisplayTime();

            if (currentMatchTimer <= 0)
            {
                StopMatch();
            }
        }
    }

    public override void StartThisState()
    {
        base.StartThisState();

        StartMatch();
    }

    public override void EndThisState()
    {
        base.EndThisState();
    }


    private void StartMatch()
    {
        // Start our timer.
        currentMatchTimer = matchTime;
        isTimerActive = true;

        // Fade in our canvas.
        CanvasFader.FadeCanvas(canvasGroup, FadeLevel.FullyOpaque, FadeSpeed.Medium);
    }

    private void StopMatch()
    {
        isTimerActive = false;

        // Disable our player.
        ClientGoblinHelper.GetMyClientGoblin().SetMovement(false);

        // Have some sort of tape over the screen to indicate the end of the round!
        stopText.SetActive(true);

        // We are done with this state!
        EndThisState();
    }

    private void DisplayTime()
    {
        timerText.text = Mathf.CeilToInt(currentMatchTimer).ToString();
    }
}
