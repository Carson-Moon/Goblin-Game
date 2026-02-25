using DG.Tweening;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GoblinCoinEating : NetworkBehaviour
{
    [SerializeField] CoinEatingUI coinEatingUI;
    [SerializeField] float eatingLength = 1;

    private GoblinCoins goblinCoins;
    
    private GrabAction grabAction;


    void Awake()
    {
        goblinCoins = GetComponent<GoblinCoins>();
        grabAction = GetComponentInChildren<GrabAction>();
    }

    public void StartEatingCoins()
    {
        if (grabAction.CurrentJar == null || grabAction.CurrentJar.Coins <= 0)
            return;

        coinEatingUI.StartEatingUI(eatingLength, EatCoin);
    }

    public void StopEatingCoins()
    {
        coinEatingUI.StopEatingUI();
    }

    private void EatCoin()
    {
        goblinCoins.GainCoin();
        grabAction.CurrentJar.LoseCoin();
    }
}
