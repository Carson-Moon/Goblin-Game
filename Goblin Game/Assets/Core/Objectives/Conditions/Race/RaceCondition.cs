using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class RaceCondition : ObjectiveCondition
{
    [SerializeField] ObjectiveZone[] rings;
    private int ringsReached = 0;

    private Dictionary<ulong, int> playerPoints = new();

    private Objective ParentObjective => GetComponentInParent<Objective>();


    public override string GetPanelDisplay()
    {
        return $"Rings: {ringsReached} / {rings.Length}";
    }

#region Setup

    protected override void OnSetupConditionServer()
    {
        // Pass
    }

    [ClientRpc]
    protected override void OnSetupConditionClientRpc()
    {
        ringsReached = 0;
        SetupRaceRings();
    }

    public override void CleanUpCondition()
    {
        HideAllRings();
    }

    public override List<ulong> GetConditionWinners()
    {
        var orderedPlayers = playerPoints.OrderByDescending(x => x.Value);
        return orderedPlayers.Where(x => x.Value == orderedPlayers.First().Value).Select(x => x.Key).ToList();
    }

#endregion

    private void SetupRaceRings()
    {
        foreach(var ring in rings)
            ring.EnableZone(OnRingReached);
    }

    private void HideAllRings()
    {
        foreach(var ring in rings)
            HideRing(ring);
    }

    private void HideRing(ObjectiveZone ring)
    {
        ring.DisableZone();
    }

    private void OnRingReached(ObjectiveZone zone)
    {
        HideRing(zone);

        ringsReached++;
        if(rings.Length == ringsReached)
            Debug.Log("Ring Objective complete!");

        UpdateConditionUI();

        OnPlayerReachedRingServerRpc(NetworkManager.Singleton.LocalClientId, ringsReached);
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnPlayerReachedRingServerRpc(ulong playerID, int ringsReached)
    {
        OnPlayerReachedRingClientRpc(playerID, ringsReached);
    }

    [ClientRpc]
    private void OnPlayerReachedRingClientRpc(ulong playerID, int ringsReached)
    {
        if(playerPoints.ContainsKey(playerID))
            playerPoints[playerID] = ringsReached;
        else
            playerPoints.Add(playerID, ringsReached);

            UpdateConditionUI();

        if(IsServer)
        {
            if(playerPoints[playerID] == rings.Length)
            {
                ParentObjective.EndObjectiveEarly();
            }
        }
    }


}
