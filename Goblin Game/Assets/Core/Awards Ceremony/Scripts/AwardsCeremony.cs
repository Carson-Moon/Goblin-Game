using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class AwardsCeremony : NetworkBehaviour
{
    [SerializeField] AwardsCeremonyStatAnnouncement statAnnouncement;

    // Announce 2 int stats and 1 float stat.
    [SerializeField] List<IntStat> intStatOptions = new();
    [SerializeField] List<FloatStat> floatStatOptions = new();


    private void Start()
    {
        if(IsServer)
            StartCoroutine(AwardsCeremonyServer());
    }

    IEnumerator AwardsCeremonyServer()
    {
        Dictionary<ulong, RoundStats> playerStats = MatchStatTracker.Instance.PlayerStats;

        statAnnouncement.IntroClientRpc();
        yield return new WaitUntil(() => !statAnnouncement.InIntro);

        for(int i=0; i<2; i++)
        {
            IntStat intStat = intStatOptions[Random.Range(0, intStatOptions.Count)];
            List<(ulong, int)> intStats = new();

            foreach(var playerStat in playerStats)
            {
                intStats.Add((playerStat.Key, playerStat.Value.GetIntStat(intStat)));
            }

            intStats = intStats.OrderByDescending(x => x.Item2).ToList();

            if(intStats.Count == 0)
            {
                Debug.LogWarning($"Did not find enough players for int stat: {intStat}.");
                continue;
            }

            statAnnouncement.AnnounceStatClientRpc(intStat.ToString().Replace('_', ' '), intStats.First().Item1.GetUsername(), intStats.First().Item2);
            yield return new WaitUntil(() => !statAnnouncement.AnnouncingStat);
        }

        yield break;
    }
}
