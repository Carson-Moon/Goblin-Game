using DG.Tweening;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GoblinCoinEating : NetworkBehaviour
{
    [SerializeField] int coinsEaten;
    public int CoinsEaten => coinsEaten;

    [Header("Movement Speed Tweaks")]
    [SerializeField] float lowestPossibleSpeed;
    [SerializeField] float highestPossibleSpeed;
    [SerializeField] int coinHighLimit;

    [Header("Eating Progress")]
    [SerializeField] private bool eating = false;
    [SerializeField] float eatingLength;
    private float eatingTimer;

    [Header("Eating UI")]
    [SerializeField] Slider progressSlider;
    [SerializeField] CanvasGroup eatingCanvasGroup;
    [SerializeField] TextMeshProUGUI coinsEatenDisplay;

    [SerializeField] GoblinCharacter goblinCharacter;
    private GrabAction grabAction;


    void Awake()
    {
        grabAction = GetComponentInChildren<GrabAction>();
        UpdateCoinsEatenDisplay();
    }

    void Update()
    {
        if(eating)
        {
            eatingTimer -= Time.deltaTime;

            if(eatingTimer <= 0)
            {
                EatCoin();
                ResetEatingTimer();
            }

            UpdateEatingUI(eatingTimer, eatingLength);
        }   
    }

    public void StartEatingCoins()
    {
        if (grabAction.CurrentJar == null || grabAction.CurrentJar.Coins <= 0)
            return;

        ToggleEatingMovementLock(true);

        ResetEatingTimer();
        eating = true;

        eatingCanvasGroup.DOFade(1, .2f);
    }

    public void StopEatingCoins()
    {
        eating = false;

        ToggleEatingMovementLock(false);

        eatingCanvasGroup.DOFade(0, .1f);
    }

    private void EatCoin()
    {
        if (!eating)
            return;

        if(grabAction.CurrentJar != null && grabAction.CurrentJar.Coins > 0)
        {
            GainEatenCoinClientRpc();
            grabAction.CurrentJar.LoseCoin();
            UpdateCoinsEatenDisplay();
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void GainEatenCoinClientRpc()
    {
        coinsEaten++;

        if(IsOwner)
            CalculateCurrentSpeed();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SetEatenCoinsClientRpc(int coins)
    {
        coinsEaten = coins;

        if(IsOwner)
            CalculateCurrentSpeed();
    }

    private void UpdateCoinsEatenDisplay()
    {
        coinsEatenDisplay.text = $"{coinsEaten}";
    }

    public int SubtractFromCoinsEaten(int amountToSubtract)
    {
        coinsEaten -= amountToSubtract;

        int coinsSubtracted = amountToSubtract;
        if(coinsEaten < 0)
        {
            coinsSubtracted = amountToSubtract + coinsEaten;
            coinsEaten = 0;
        }

        SetEatenCoinsClientRpc(coinsEaten);
        UpdateCoinsEatenDisplay();
        CalculateCurrentSpeed();

        return coinsSubtracted;  
    }
    
    private void ResetEatingTimer()
    {
        eatingTimer = eatingLength;
    }

    private void CalculateCurrentSpeed()
    {
        float speedPercentage = 1f - ((float)coinsEaten / coinHighLimit);
        speedPercentage = Mathf.Clamp(speedPercentage, 0, 1);

        float desiredSpeed = highestPossibleSpeed * speedPercentage;
        desiredSpeed = Mathf.Clamp(desiredSpeed, lowestPossibleSpeed, highestPossibleSpeed);

        goblinCharacter.SetWalkSpeed(desiredSpeed);
    }

    private void ToggleEatingMovementLock(bool value)
    {
        goblinCharacter.eatingMovementLock = value;
    }
    
    #region Eating UI
    private void UpdateEatingUI(float current, float total)
    {
        progressSlider.value = 1f - (current / total);
    }
    #endregion
}
