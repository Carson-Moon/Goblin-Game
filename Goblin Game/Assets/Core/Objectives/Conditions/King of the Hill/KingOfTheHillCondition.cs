using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KingOfTheHillCondition : ObjectiveCondition
{
    [SerializeField] KingOfTheHillZone[] zones;
    [SerializeField] float zoneTime;
    private Dictionary<ulong, float> playerPoints = new();
    private int zoneIndex = 0;


    public override string GetPanelDisplay()
    {
        if(playerPoints.ContainsKey(NetworkManager.Singleton.LocalClientId))
            return $"Zone Points: {playerPoints[NetworkManager.Singleton.LocalClientId].ToString("F2")}";
        else
            return $"Zone Points: 0";
    }

    
#region Setup

    protected override void OnSetupConditionServer()
    {
        StartZoneTimerServer();
    }

    

    [ClientRpc]
    protected override void OnSetupConditionClientRpc()
    {
        zones[zoneIndex].EnableZone(OnLocalPlayerUpdatePoints);
    }

#endregion

    private void StartZoneTimerServer()
    {
        StartCoroutine(ZoneSwitchTimer());
    }

    IEnumerator ZoneSwitchTimer()
    {
        yield return new WaitForSeconds(zoneTime);

        zoneIndex++;
        if(zoneIndex == zones.Length)
            zoneIndex = 0;

        SwitchZoneClientRpc(zoneIndex);

        StartZoneTimerServer();
    }

    [ClientRpc]
    private void SwitchZoneClientRpc(int zoneIndex)
    {
        foreach(var zone in zones)
            zone.DisableZone();

        zones[zoneIndex].EnableZone(OnLocalPlayerUpdatePoints);
    }

    private void OnLocalPlayerUpdatePoints(ulong playerID)
    {
        float totalPoints = 0;
        foreach(var zone in zones)
            totalPoints += zone.PointsGathered;
        
        OnPlayerUpdatePointsServerRpc(playerID, totalPoints);
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnPlayerUpdatePointsServerRpc(ulong playerID, float points)
    {
        OnPlayerUpdatePointsClientRpc(playerID, points);
    }

    [ClientRpc]
    private void OnPlayerUpdatePointsClientRpc(ulong playerID, float points)
    {
        if(playerPoints.ContainsKey(playerID))
            playerPoints[playerID] = points;
        else
            playerPoints.Add(playerID, points);

        Debug.Log($"{playerID} has {points} points.");

        UpdateConditionUI();
    }
}
