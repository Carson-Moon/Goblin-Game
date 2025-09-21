using DG.Tweening;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameplayState : GameState
{
    [SerializeField] CanvasGroup initialCountdownGroup;
    [SerializeField] TextMeshProUGUI initialCountdownText;

    [SerializeField] bool countdown = false;
    [SerializeField] float countdownLength;
    [SerializeField] float currentCountdown;
    [SerializeField] CanvasGroup countdownGroup;
    [SerializeField] TextMeshProUGUI countdownText;


    protected override void OnStartStateServer()
    {
        OnStartStateClientRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    public override void OnStartStateClientRpc()
    {
        CreateLock();

        // Initial 3 second countdown.
        Sequence sequence = DOTween.Sequence();
        sequence.Append(PerformInitialCountdown());
        sequence.AppendCallback(() =>
        {
            currentCountdown = countdownLength;
            countdownText.text = currentCountdown.ToString("F0");
            countdownGroup.alpha = 1;

            GoblinClientPointer.LocalGoblinClient().ToggleMovement(true);

            countdown = true;
        });
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

                OnCountdownEnd();
            }
        }
    }

    private void OnCountdownEnd()
    {
        Debug.Log("Countdown ended!");

        GoblinClientPointer.LocalGoblinClient().ToggleMovement(false);

        // Endscreen tape!

        ClientUnlock();
    }

    private Tween PerformInitialCountdown()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(InitialCountdownNumber("3"));
        sequence.Append(InitialCountdownNumber("2"));
        sequence.Append(InitialCountdownNumber("1"));
        sequence.Append(InitialCountdownNumber("GO!"));

        return sequence;
    }

    private Tween InitialCountdownNumber(string numText)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            initialCountdownText.text = numText;
            initialCountdownGroup.alpha = 1;
        });
        sequence.Append(initialCountdownGroup.DOFade(0, 1f));

        return sequence;
    }
}
