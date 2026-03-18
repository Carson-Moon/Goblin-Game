using System.Linq;
using UnityEngine;

public static class NetworkExtensions
{
    public static string GetUsername(this ulong id)
    {
        if(ServerLobbyManager.Instance != null)
        {
            if(ServerLobbyManager.Instance.PlayerInformations.TryGetValue(id, out PlayerInformation playerInfo))
                return playerInfo.Username.ToString();
            
            Debug.LogWarning($"Could not find player with id {id}.");
            return default;
        }
        
        Debug.LogWarning("Could not find ServerLobbyManager!");
        return default;
    }
}
