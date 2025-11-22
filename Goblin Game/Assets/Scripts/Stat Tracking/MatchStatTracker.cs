using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Gathers everyones stats at the end of each round.

public class MatchStatTracker : NetworkBehaviour
{
    // Singleton
    public static MatchStatTracker instance { get; private set; }

    [SerializeField] Dictionary<ulong, RoundStats> playerStats = new Dictionary<ulong, RoundStats>();
    public Dictionary<ulong, RoundStats> PlayerStats => playerStats;


    void Awake()
    {
        // Singleton Setup
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Save our stats to the dictionary. If we already have stats, condense them.
    [Rpc(SendTo.Server)]
    public void SaveClientStatsServerRpc(ulong clientID, RoundStats stats)
    {
        // Determine if we already have stats saved for this client.
        if (playerStats.TryGetValue(clientID, out RoundStats savedStats))
        {
            // Condense our stats.
            RoundStats newStats = CondenseClientStats(savedStats, stats);
            playerStats[clientID] = newStats;
            SaveClientStatsClientRpc(clientID, newStats);
            return;
        }
        else
        {
            // Save our stats.
            playerStats.Add(clientID, stats);
            SaveClientStatsClientRpc(clientID, stats);
        }  
    }

    [Rpc(SendTo.NotServer)]
    public void SaveClientStatsClientRpc(ulong clientID, RoundStats stats)
    {
        if(playerStats.ContainsKey(clientID))
            playerStats[clientID] = stats;
        else
            playerStats.Add(clientID, stats);

    }

    // Condense our stats into one RoundStats struct. Save to this clientID.
    public RoundStats CondenseClientStats(RoundStats oldStats, RoundStats newStats)
    {
        // Create a new stats struct and add our stats together.
        return new RoundStats()
        {
            CoinsCollected = oldStats.CoinsCollected + newStats.CoinsCollected,
            TotalCoinsAtEnd = oldStats.TotalCoinsAtEnd + newStats.TotalCoinsAtEnd,
            TimesStabbedOtherPlayers = oldStats.TimesStabbedOtherPlayers + newStats.TimesStabbedOtherPlayers,
            TimesStabbedByOtherPlayer = oldStats.TimesStabbedByOtherPlayer + newStats.TimesStabbedByOtherPlayer,
            TimeKnockedOut = oldStats.TimeKnockedOut + newStats.TimeKnockedOut,
            TimesKnockedOutOtherPlayer = oldStats.TimesKnockedOutOtherPlayer + newStats.TimesKnockedOutOtherPlayer,
            TimesKnockedOutByOtherPlayer = oldStats.TimesKnockedOutByOtherPlayer + newStats.TimesKnockedOutByOtherPlayer,
            TimeCrouching = oldStats.TimeCrouching + newStats.TimeCrouching,
            TimeSprinting = oldStats.TimeSprinting + newStats.TimeSprinting,
            TimeDoingNothing = oldStats.TimeDoingNothing + newStats.TimeDoingNothing
        };
    }
}
