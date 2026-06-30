using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CeremonyStatAnnouncement : MonoBehaviour
{
    [SerializeField] CeremonyWheel wheelUI;


    public void StartAnnouncingStat(string stat, Action<ulong> onComplete)
    {
        StartCoroutine(AnnounceStat(stat, onComplete));
    }

    IEnumerator AnnounceStat(string stat, Action<ulong> onComplete)
    {
        // Spin the wheel.

        // Announce the winner.

        // Done!
        yield return new WaitForSeconds(1f);
        onComplete?.Invoke(NetworkManager.Singleton.LocalClientId);
    }
}
