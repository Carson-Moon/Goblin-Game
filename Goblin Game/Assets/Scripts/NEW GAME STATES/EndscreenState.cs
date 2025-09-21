using TMPro;
using Unity.Netcode;
using UnityEngine;

public class EndscreenState : GameState
{
    [SerializeField] CanvasGroup endscreenGroup;
    [SerializeField] Animated_Button animatedButton;

    [SerializeField] bool countdown = false;
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] float countdownLength;
    [SerializeField] float currentCountdown;


    protected override void OnStartStateServer()
    {
        CreateLock();

        OnStartStateClientRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    public override void OnStartStateClientRpc()
    {
        // This function will receive the chart data or whatever we
        // want to display on the endscreen here and set it up.
        // Probably want to do a little animation.

        currentCountdown = countdownLength;
        countdownText.text = currentCountdown.ToString("F2");
        countdown = true;

        endscreenGroup.interactable = true;
        endscreenGroup.alpha = 1;
        animatedButton.gameObject.SetActive(true);
    }

    void Update()
    {
        if (countdown)
        {
            currentCountdown -= Time.deltaTime;
            countdownText.text = currentCountdown.ToString("F0");

            if (currentCountdown <= 0)
            {
                countdown = false;
                currentCountdown = 0;
                countdownText.text = "0";

                // THIS SHOULD BE A DIFFERENT FUNCTION THAT DISABLES THE NEXT BUTTON BY MAKING IT UNINTERACTABLE.
                OnNextPressed();
            }
        }
    }

    public void OnNextPressed()
    {
        // DEFINITELY NEED TO MAKE THE BUTTON UNINTERACTABLE AFTER THIS PRESS.
        animatedButton.gameObject.SetActive(false);

        ClientUnlock();
    }

    protected override void OnEndStateServer()
    {
        ServerStartGame.Instance.AttemptTransitionToGameScene();
    }
}
