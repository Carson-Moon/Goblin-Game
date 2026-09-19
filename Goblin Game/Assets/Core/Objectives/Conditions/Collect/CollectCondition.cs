using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CollectCondition : ObjectiveCondition
{
    [SerializeField] ObjectiveZone[] collectables;
    private Dictionary<ulong, int> playerPoints = new();


    public override string GetPanelDisplay()
    {
        if(playerPoints.ContainsKey(NetworkManager.Singleton.LocalClientId))
            return $"Collected: {playerPoints[NetworkManager.Singleton.LocalClientId]}";
        else
            return "Collected: 0";
    }

#region Setup

    protected override void OnSetupConditionServer()
    {
        // pass
    }

    [ClientRpc]
    protected override void OnSetupConditionClientRpc()
    {
        playerPoints.Clear();

        foreach(var collectable in collectables)
            collectable.EnableZone(OnLocalPlayerEnteredZone);
    }

#endregion

    private void OnLocalPlayerEnteredZone(ObjectiveZone zone)
    {
        OnPlayerEnteredZoneServerRpc(NetworkManager.Singleton.LocalClientId, Array.IndexOf(collectables, zone));
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnPlayerEnteredZoneServerRpc(ulong playerID, int collectableIndex)
    {
        int currentPoints = (playerPoints.ContainsKey(playerID) ? playerPoints[playerID] : 0) + 1;

        OnPlayerEnteredZoneClientRpc(playerID, currentPoints, collectableIndex);
    }

    [ClientRpc]
    private void OnPlayerEnteredZoneClientRpc(ulong playerID, int points, int collectableIndex)
    {
        if(collectableIndex >= 0 && collectableIndex < collectables.Length)
            collectables[collectableIndex].DisableZone();

        if(playerPoints.ContainsKey(playerID))
            playerPoints[playerID] = points;
        else
            playerPoints.Add(playerID, points);

        // Debug.Log($"{playerID} has {points} points.");

        UpdateConditionUI();
    }
}
