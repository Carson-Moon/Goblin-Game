using System;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

public class RaceDestination : MonoBehaviour
{
    private bool listening = false;

    public event Action<ulong> OnLocalPlayerEntered;


    public void Initialize(Action<ulong> onComplete)
    {
        listening = true;
        OnLocalPlayerEntered += onComplete;
    }

    public void Clear(Action<ulong> onComplete)
    {
        listening = false;
        OnLocalPlayerEntered -= onComplete;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!listening)
            return;

        NetworkObject networkObject = other.gameObject.GetComponentInParent<NetworkObject>();
        if(networkObject != null && networkObject.OwnerClientId == NetworkManager.Singleton.LocalClientId)     // Goblin Layer
        {
            listening = false;
            OnLocalPlayerEntered?.Invoke(NetworkManager.Singleton.LocalClientId);
            Debug.Log("Goblin entered!");
        }
    }
}
