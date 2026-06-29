using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CeremonyStatAnnouncementState : CeremonyState
{
    [SerializeField] CeremonyWheel wheelUI;

    private Dictionary<ulong, bool> playersAnnouncingStat = new();
    private bool waitingForPlayers = false;
    


    public override void StartState()
    {
        base.StartState();


    }

    IEnumerator StatAnnounceSequence()
    {
        AnnounceStatClientRpc();

        yield return null;
    }

    [ClientRpc]
    private void AnnounceStatClientRpc()
    {
        
    }

    IEnumerator AnnounceStat()
    {
        yield return null;
    }

    [ServerRpc]
    private void DoneAnnouncingStatServerRpc(ulong playerID)
    {
        playersAnnouncingStat[playerID] = true;

        bool allPlayersDone = true;
        foreach(var kvp in playersAnnouncingStat)
        {
            if(!kvp.Value)
                allPlayersDone = false;
        }

        // if(allPlayersDone)

    }
}
