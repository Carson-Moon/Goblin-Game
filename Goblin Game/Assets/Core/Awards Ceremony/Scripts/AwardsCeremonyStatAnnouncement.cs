using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AwardsCeremonyStatAnnouncement : NetworkBehaviour
{
    private bool inIntro = false;
    public bool InIntro => inIntro;

    private bool announcingStat = false;
    public bool AnnouncingStat => announcingStat;

    [ClientRpc]
    public void IntroClientRpc()
    {
        inIntro = true;

        StartCoroutine(Intro());
    }

    IEnumerator Intro()
    {
        inIntro = false;

        yield break;
    }

    [ClientRpc]
    public void AnnounceStatClientRpc(string statTitle, string winnerName, int stat)
    {
        announcingStat = true;

        StartCoroutine(AnnounceIntStat(statTitle, winnerName, stat));
    }

    private void StopAnnouncingStat()
    {
        announcingStat = false;
    }

    IEnumerator AnnounceIntStat(string statTitle, string winnerName, int stat)
    {
        Debug.Log("Drumroll please...");

        yield return new WaitForSeconds(2);

        Debug.Log($"The winner of {statTitle} is {winnerName} with {stat}!");

        yield return new WaitForSeconds(3);

        StopAnnouncingStat();

        yield break;
    }

    IEnumerator AnnounceFloatStat(string statTitle, string winnerName, List<(ulong, float)> stats)
    {
        yield break;   
    }
}
