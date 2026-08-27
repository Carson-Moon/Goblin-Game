using System;
using Unity.Netcode;
using UnityEngine;

public class ObjectiveZone : MonoBehaviour
{
    private bool listeningForPlayer = false;

    public event Action OnLocalPlayerEntered;


    void Awake()
    {
        DisableZone();
    }

    public void EnableZone(Action onComplete)
    {
        listeningForPlayer = true;
        OnLocalPlayerEntered += onComplete;
        gameObject.SetActive(true);
    }

    public void DisableZone()
    {
        listeningForPlayer = false;
        OnLocalPlayerEntered = null;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(!listeningForPlayer)
            return;

        // For local testing, think of a more elegant solution?
        if(NetworkManager.Singleton == null && other.TryGetComponent(out GoblinCharacter _))
        {
            OnLocalPlayerEntered?.Invoke();
            listeningForPlayer = false;
            DisableZone();
            return;
        }

        NetworkObject networkObject = other.gameObject.GetComponentInParent<NetworkObject>();
        if(networkObject != null && networkObject.OwnerClientId == NetworkManager.Singleton.LocalClientId)
        {
            OnLocalPlayerEntered?.Invoke();
            listeningForPlayer = false;
            DisableZone();
        }
    }
}
