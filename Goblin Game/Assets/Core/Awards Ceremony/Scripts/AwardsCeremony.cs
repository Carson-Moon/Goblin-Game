using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using WebSocketSharp;

public class AwardsCeremony : NetworkBehaviour
{
    [SerializeField] AwardsCeremonyStatAnnouncement statAnnouncement;
    [SerializeField] WinnersWheel wheel;

    [SerializeField] SpawnPointSetter spawnPointSetter;

    // Announce 2 int stats and 1 float stat.
    [SerializeField] List<IntStat> intStatOptions = new();
    [SerializeField] List<FloatStat> floatStatOptions = new();

    private Dictionary<ulong, int> finalPoints = new();


    private void Start()
    {
        if(IsServer)
            StartCoroutine(AwardsCeremonyServer());
    }

    IEnumerator AwardsCeremonyServer()
    {
        spawnPointSetter.MovePlayersToSpawnPoints();

        yield return new WaitForSeconds(3f);

        Dictionary<ulong, RoundStats> playerStats = MatchStatTracker.Instance.PlayerStats;

        statAnnouncement.IntroClientRpc(intStatOptions.Select(x => (int)x).ToArray(), floatStatOptions.Select(x => (int)x).ToArray());
        yield return new WaitUntil(() => !statAnnouncement.InIntro);

        for(int i=0; i<2; i++)
        {
            int category = wheel.ChooseCategory();

            List<(ulong, int)> intStats = new();
            foreach(var playerStat in playerStats)
            {
                // intStats.Add((playerStat.Key, playerStat.Value.GetIntStat(intStat)));
            }
            intStats = intStats.OrderByDescending(x => x.Item2).ToList();

            if(intStats.Count == 0)
            {
                // Debug.LogWarning($"Did not find enough players for int stat: {intStat}.");
                continue;
            }

            if(finalPoints.ContainsKey(intStats.First().Item1))
                finalPoints[intStats.First().Item1]++;
            else
                finalPoints.Add(intStats.First().Item1, 1);

            // statAnnouncement.AnnounceStatClientRpc(intStat.ToString().Replace('_', ' '), intStats.First().Item1.GetUsername(), intStats.First().Item2);
            yield return new WaitUntil(() => !statAnnouncement.AnnouncingStat);
            yield return new WaitForSeconds(2f);
        }

        for(int i=0; i<1; i++)
        {
            List<FloatStat> statOptions = new(floatStatOptions);
            FloatStat floatStat = statOptions[Random.Range(0, statOptions.Count)];
            statOptions.Remove(floatStat);

            List<(ulong, float)> floatStats = new();
            foreach(var playerStat in playerStats)
            {
                floatStats.Add((playerStat.Key, playerStat.Value.GetFloatStat(floatStat)));
            }
            floatStats = floatStats.OrderByDescending(x => x.Item2).ToList();

            if(floatStats.Count == 0)
            {
                Debug.LogWarning($"Did not find enough players for float stat: {floatStat}.");
                continue;
            }

            if(finalPoints.ContainsKey(floatStats.First().Item1))
                finalPoints[floatStats.First().Item1]++;
            else
                finalPoints.Add(floatStats.First().Item1, 1);

            statAnnouncement.AnnounceStatClientRpc(floatStat.ToString().Replace('_', ' '), floatStats.First().Item1.GetUsername(), (int)floatStats.First().Item2);
            yield return new WaitUntil(() => !statAnnouncement.AnnouncingStat);
            yield return new WaitForSeconds(2f);
        }

        ulong winnerID = finalPoints.OrderByDescending(x => x.Value).First().Key;

        statAnnouncement.AnnounceWinnerClientRpc(winnerID.GetUsername());

        yield break;
    }
}
