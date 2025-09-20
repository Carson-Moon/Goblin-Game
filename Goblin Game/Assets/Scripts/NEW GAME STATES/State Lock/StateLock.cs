using System;
using System.Collections.Generic;
using UnityEngine;

public class StateLock
{
    public event Action OnUnlock = null;
    private Dictionary<ulong, bool> clientLocks = new();


    public StateLock(List<ulong> clientIDs, Action unlockAction)
    {
        OnUnlock += unlockAction;

        clientLocks.Clear();
        foreach (ulong clientID in clientIDs)
        {
            clientLocks.Add(clientID, false);
        }
    }

    public void ReceiveClientUnlock(ulong clientID)
    {
        clientLocks[clientID] = true;

        foreach (KeyValuePair<ulong, bool> clientLock in clientLocks)
        {
            if (clientLock.Value == false)
                return;
        }

        // We are now unlocked.
        Debug.Log("Unlocked!");
        OnUnlock?.Invoke();
    }
}
