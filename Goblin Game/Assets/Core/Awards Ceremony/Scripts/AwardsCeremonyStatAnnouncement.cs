using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AwardsCeremonyStatAnnouncement : NetworkBehaviour
{
    [SerializeField] AwardsCeremonyUI awardsCeremonyUI;

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
        awardsCeremonyUI.ResetScreen();

        announcingStat = false;
    }

    IEnumerator AnnounceIntStat(string statTitle, string winnerName, int stat)
    {
        Debug.Log("Drumroll please...");
        awardsCeremonyUI.SetStatTitle(statTitle);

        yield return new WaitForSeconds(2);

        Debug.Log($"The winner of {statTitle} is {winnerName} with {stat}!");
        awardsCeremonyUI.SetPlayerName(winnerName);

        yield return new WaitForSeconds(3);

        StopAnnouncingStat();

        yield break;
    }

    [ClientRpc]
    public void AnnounceWinnerClientRpc(string winnerName)
    {
        StartCoroutine(AnnounceWinner(winnerName));
    }

    IEnumerator AnnounceWinner(string winnerName)
    {
        Debug.Log("Drumroll please...");
        awardsCeremonyUI.SetStatTitle("The winner is...");

        yield return new WaitForSeconds(2);

        Debug.Log($"The overall winner is {winnerName}!");
        awardsCeremonyUI.SetPlayerName(winnerName);
    }
}
