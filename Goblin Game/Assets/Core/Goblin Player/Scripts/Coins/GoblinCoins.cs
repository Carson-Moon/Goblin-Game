using System;
using Unity.Netcode;
using UnityEngine;

public class GoblinCoins : NetworkBehaviour
{
    [SerializeField] private int _coins;
    public int Coins => _coins;

    [SerializeField] private int _maxCoins;
    public int MaxCoins => _maxCoins;

    [Header("UI")]
    [SerializeField] CoinFillUI coinFillUI;

    public event Action<int> OnNumberOfCoinsChanged;


    void Start()
    {
        Initialize(_maxCoins);
    }

    public void Initialize(int maxCoins)
    {
        _maxCoins = maxCoins;

        coinFillUI.Initialize(_maxCoins);
    }

    [ServerRpc(RequireOwnership = false)]
    public void GainCoinServerRpc()
    {
        GainCoinClientRpc();
    }

    [ClientRpc]
    private void GainCoinClientRpc()
    {
        _coins++;

        if(IsOwner)
        {
            coinFillUI.UpdateUI(_coins);
            OnNumberOfCoinsChanged?.Invoke(_coins);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void LoseCoinServerRpc()
    {
        LoseCoinClientRpc();
    }

    [ClientRpc]
    private void LoseCoinClientRpc()
    {
        _coins--;

        if(IsOwner)
        {
            coinFillUI.UpdateUI(_coins);

            OnNumberOfCoinsChanged?.Invoke(_coins);
        }
    }

    public int LoseCoins(int coinsToLose)
    {
        if(_coins >= coinsToLose)
        {
            _coins -= coinsToLose;
            coinFillUI.UpdateUI(_coins);
            return coinsToLose;
        }
        else
        {
            int coinsLost = _coins;
            _coins = 0;
            coinFillUI.UpdateUI(_coins);
            return coinsLost;
        }
    }
}
