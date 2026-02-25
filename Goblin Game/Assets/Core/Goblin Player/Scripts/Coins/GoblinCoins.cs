using System;
using UnityEngine;

public class GoblinCoins : MonoBehaviour
{
    [SerializeField] private int _coins;
    public int Coins => _coins;

    [SerializeField] private int _maxCoins;
    public int MaxCoins => _maxCoins;

    [Header("UI")]
    [SerializeField] CoinFillUI coinFillUI;

    public event Action<int> OnNumberOfCoinsChanged;

    private GoblinCharacter goblinCharacter;

    void Start()
    {
        Initialize(_maxCoins);
    }

    public void Initialize(int maxCoins)
    {
        _maxCoins = maxCoins;

        coinFillUI.Initialize(_maxCoins);
    }

    public void GainCoin()
    {
        _coins++;
        coinFillUI.UpdateUI(_coins);

        OnNumberOfCoinsChanged?.Invoke(_coins);
    }

    public void LoseCoin()
    {
        _coins--;
        coinFillUI.UpdateUI(_coins);

        OnNumberOfCoinsChanged?.Invoke(_coins);
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
