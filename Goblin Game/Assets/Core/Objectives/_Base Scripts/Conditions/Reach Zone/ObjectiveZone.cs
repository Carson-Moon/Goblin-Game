using System;
using Unity.Netcode;
using UnityEngine;

public class ObjectiveZone : MonoBehaviour
{
    private bool listeningForPlayer = false;

    public event Action OnLocalPlayerEntered;


    public void EnableZone(Action onComplete)
    {
        listeningForPlayer = true;
        OnLocalPlayerEntered += onComplete;
    }

    public void DisableZone()
    {
        listeningForPlayer = false;
        OnLocalPlayerEntered = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if(!listeningForPlayer)
            return;

        NetworkObject networkObject = other.gameObject.GetComponentInParent<NetworkObject>();
        if(networkObject != null && networkObject.OwnerClientId == NetworkManager.Singleton.LocalClientId)
        {
            OnLocalPlayerEntered?.Invoke();
            listeningForPlayer = false;
            DisableZone();
        }
    }
}
