using System;
using Unity.Netcode;
using UnityEngine;

public class PickupCoins : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<int> _coins = new NetworkVariable<int>(0);
    public int Coins => _coins.Value;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        _coins.OnValueChanged += OnValueChanged;
    }

    private void OnValueChanged(int previousValue, int newValue)
    {
        // We can play a vfx here if coins are lost or gained.
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddCoinsServerRpc(int coinsToAdd)
    {
        _coins.Value += coinsToAdd;
    }

    [ServerRpc(RequireOwnership = false)]
    public void LoseCoinsServerRpc(int coinsToLose)
    {
        _coins.Value -= coinsToLose;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetCoinsServerRpc(int coinsToSet)
    {
        _coins.Value = coinsToSet;
    }
}
