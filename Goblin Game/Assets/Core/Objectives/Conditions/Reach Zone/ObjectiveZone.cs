using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class ObjectiveZone : MonoBehaviour
{
    [SerializeField] float disableDelay = 0;
    private bool listeningForPlayer = false;

    public event Action OnLocalPlayerEntered;


    void Awake()
    {
        DisableZone(true);
    }

    public void EnableZone(Action onComplete)
    {
        listeningForPlayer = true;
        OnLocalPlayerEntered = null;
        OnLocalPlayerEntered?.Invoke();

        OnLocalPlayerEntered += onComplete;
        gameObject.SetActive(true);
    }

    public void DisableZone(bool skipDelay = false)
    {
        listeningForPlayer = false;
        OnLocalPlayerEntered = null;
        StartCoroutine(DisableGameObjectDelay(skipDelay));
    }

    IEnumerator DisableGameObjectDelay(bool skipDelay = false)
    {
        if(!skipDelay)
            yield return new WaitForSeconds(disableDelay);
        
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
        if(networkObject != null && NetworkManager.Singleton != null && networkObject.OwnerClientId == NetworkManager.Singleton.LocalClientId)
        {
            OnLocalPlayerEntered?.Invoke();
            listeningForPlayer = false;
            DisableZone();
        }
    }
}
