using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CeremonyStatAnnouncement : MonoBehaviour
{
    [SerializeField] CeremonyWheel wheelUI;
    private bool wheelIsSpinning = false;


    public void StartAnnouncingStat(string stat, Action<ulong> onComplete)
    {
        StartCoroutine(AnnounceStat(stat, onComplete));
    }

    IEnumerator AnnounceStat(string stat, Action<ulong> onComplete)
    {
        // Spin the wheel.
        wheelIsSpinning = true;
        wheelUI.Spin(stat, () => wheelIsSpinning = false);
        yield return new WaitUntil(() => !wheelIsSpinning);

        // Announce the winner.
        Debug.Log("Winner is...");

        // Done!
        yield return new WaitForSeconds(1f);
        onComplete?.Invoke(NetworkManager.Singleton.LocalClientId);
    }
}
