using System;
using Unity.Netcode;
using UnityEngine;

public class KingOfTheHillZone : MonoBehaviour
{
    [SerializeField] private float pointsGathered;
    public float PointsGathered => pointsGathered;
    private bool gatherPoints = false;

    private float sendPointsTimer;
    [SerializeField] private float sendPointsTimerLength = 0.5f;

    private Action<ulong> OnSendPoints;


    void Awake()
    {
        DisableZone();
    }

    public void EnableZone(Action<ulong> onSendPoints)
    {
        OnSendPoints += onSendPoints;
        sendPointsTimer = sendPointsTimerLength;
        gameObject.SetActive(true);
    }

    public void DisableZone()
    {
        OnSendPoints = null;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if(gatherPoints)
        {
            pointsGathered += Time.deltaTime;

            sendPointsTimer -= Time.deltaTime;
            if(sendPointsTimer <= 0)
            {
                OnSendPoints?.Invoke(NetworkManager.Singleton.LocalClientId);
                sendPointsTimer = sendPointsTimerLength;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        NetworkObject networkObject = other.gameObject.GetComponentInParent<NetworkObject>();
        if(networkObject != null && NetworkManager.Singleton != null && networkObject.OwnerClientId == NetworkManager.Singleton.LocalClientId)
            gatherPoints = true;
    }

    void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out GoblinCharacter goblin))
        {
            NetworkObject networkObject = goblin.gameObject.GetComponentInParent<NetworkObject>();
            if(networkObject != null && NetworkManager.Singleton != null && networkObject.OwnerClientId == NetworkManager.Singleton.LocalClientId)
            {
                gatherPoints = false;
                OnSendPoints?.Invoke(NetworkManager.Singleton.LocalClientId);
            }
        }
        
    }
}
