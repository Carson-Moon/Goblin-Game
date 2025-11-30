using DG.Tweening;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GoblinCoinEating : NetworkBehaviour
{
    [SerializeField] int coinsEaten;
    public int CoinsEaten => coinsEaten;

    [Header("Eating Progress")]
    [SerializeField] private bool eating = false;
    [SerializeField] float eatingLength;
    [SerializeField] private float eatingTimer;

    [Header("Eating UI")]
    [SerializeField] Slider progressSlider;
    [SerializeField] CanvasGroup eatingCanvasGroup;
    [SerializeField] TextMeshProUGUI coinsEatenDisplay;

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

        ResetEatingTimer();
        eating = true;

        eatingCanvasGroup.DOFade(1, .2f);
    }

    public void StopEatingCoins()
    {
        eating = false;

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
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SetEatenCoinsClientRpc(int coins)
    {
        coinsEaten = coins;
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

        return coinsSubtracted;  
    }
    
    private void ResetEatingTimer()
    {
        eatingTimer = eatingLength;
    }
    
    #region Eating UI
    private void UpdateEatingUI(float current, float total)
    {
        progressSlider.value = 1f - (current / total);
    }
    #endregion
}
