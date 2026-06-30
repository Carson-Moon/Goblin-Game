using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class CeremonyStatAnnouncementState : CeremonyState
{
    [SerializeField] CeremonySetupState setupState;
    [SerializeField] CeremonyStatAnnouncement statAnnouncement;
    [SerializeField] int numStatsToAnnounce;

    private List<string> usedStats = new();
    private Dictionary<ulong, bool> playersDone = new();
    private bool waitingForPlayers = false;
    


    public override void StartState()
    {
        base.StartState();

        StartCoroutine(StatAnnounceSequence());
    }

    private string GetRandomStat()
    {
        List<string> availableStats = setupState.AllStatStrings.Where(x => !usedStats.Contains(x)).ToList();
        string randomStat = availableStats[Random.Range(0, availableStats.Count)];
        usedStats.Add(randomStat);
        return randomStat;
    }

    IEnumerator StatAnnounceSequence()
    {
        for(int i=0; i<numStatsToAnnounce; i++)
        {
            Debug.Log($"Stat number {i+1}!");

            playersDone.Clear();
            foreach(ulong clientID in ServerLobbyManager.Instance.ClientIDs)
                playersDone.Add(clientID, false);

            waitingForPlayers = true;
            AnnounceStatClientRpc(GetRandomStat());
            yield return new WaitUntil(() => !waitingForPlayers);
        }

        EndState();
    }

    [ClientRpc]
    private void AnnounceStatClientRpc(string statToAnnounce)
    {
        statAnnouncement.StartAnnouncingStat(statToAnnounce, DoneAnnouncingStatServerRpc);
    }

    [ServerRpc]
    private void DoneAnnouncingStatServerRpc(ulong playerID)
    {
        playersDone[playerID] = true;

        bool allPlayersDone = true;
        foreach(var kvp in playersDone)
        {
            if(!kvp.Value)
                allPlayersDone = false;
        }

        if(allPlayersDone)
            waitingForPlayers = false;

    }
}
