using System;
using Unity.Netcode;
using UnityEngine;

public class GameState : NetworkBehaviour
{
    [SerializeField] GameState nextState;
    protected Action<ulong> OnComplete;

    // Lock
    protected StateLock stateLock = null;
    protected Action OnClientUnlocked = null;

    [Rpc(SendTo.Server)]
    public void OnStartStateServerRpc()
    {
        OnStartStateServer();
    }

    [Rpc(SendTo.ClientsAndHost)]
    public virtual void OnStartStateClientRpc()
    {

    }

    protected virtual void OnStartStateServer()
    {

    }

    [Rpc(SendTo.Server)]
    public void OnEndStateServerRpc()
    {
        OnEndStateServer();
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void OnEndStateClientRpc()
    {

    }

    protected virtual void OnEndStateServer()
    {
        Debug.Log("End state...");
        nextState.OnStartStateServerRpc();
    }

    #region Locks
    public void CreateLock()
    {
        stateLock = new StateLock(ServerLobbyManager.Instance.ClientIDs, OnEndStateServerRpc);
    }

    // Clients call this themselves.
    public void ClientUnlock()
    {
        ClientUnlockServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [Rpc(SendTo.Server)]
    public void ClientUnlockServerRpc(ulong clientID)
    {
        Debug.Log($"Client unlocked: {clientID}");

        if (stateLock == null)
        {
            Debug.Log("We have a problem! Statelock is null!");
            return;
        }

        stateLock.ReceiveClientUnlock(clientID);      
    }
    #endregion
}
