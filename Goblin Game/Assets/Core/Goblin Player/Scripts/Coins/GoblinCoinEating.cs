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
    
    private PickupAction pickupAction;


    void Awake()
    {
        goblinCoins = GetComponent<GoblinCoins>();
        pickupAction = GetComponentInChildren<PickupAction>();
    }

    public void StartEatingCoins()
    {
        if (pickupAction.CurrentPickup == null || !pickupAction.CurrentPickup.HoldsCoins || pickupAction.CurrentPickup.PickupCoins.Coins == 0)
            return;

        coinEatingUI.StartEatingUI(eatingLength, EatCoin);
    }

    public void StopEatingCoins()
    {
        coinEatingUI.StopEatingUI();
    }

    private void EatCoin()
    {
        goblinCoins.GainCoinServerRpc();

        if(pickupAction.CurrentPickup.PickupCoins.Coins - 1 == 0)
            StopEatingCoins();

        pickupAction.CurrentPickup.PickupCoins.LoseCoinsServerRpc(1);
    }
}
