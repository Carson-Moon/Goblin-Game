using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Gathers everyones stats at the end of each round.

public class MatchStatTracker : NetworkBehaviour
{
    // Singleton
    public static MatchStatTracker instance { get; private set; }

    [SerializeField] Dictionary<ulong, RoundStats> playerStats = new Dictionary<ulong, RoundStats>();
    [SerializeField] RoundStats newStats;


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
    public void SaveClientStatsRPC(ulong clientID, RoundStats stats)
    {
        // Determine if we already have stats saved for this client.
        if (playerStats.TryGetValue(clientID, out RoundStats savedStats))
        {
            // Condense our stats.
            CondenseClientStats(clientID, savedStats, stats);
        }
        else
        {
            // Save our stats.
            playerStats.Add(clientID, stats);
        }

        newStats = stats;
    }

    // Condense our stats into one RoundStats struct. Save to this clientID.
    public void CondenseClientStats(ulong clientID, RoundStats oldStats, RoundStats newStats)
    {
        // Create a new stats struct and add our stats together.
        RoundStats totalStats = new RoundStats()
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

        // Update totalStats for this clientID.
        playerStats[clientID] = totalStats;
    }
}
