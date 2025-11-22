using System.Collections.Generic;
using UnityEngine;

public class EndscreenGraph : MonoBehaviour
{
    [SerializeField] GraphBarUI graphPrefab;
    [SerializeField] RectTransform barGraphHolder;
    [SerializeField] Dictionary<ulong, GraphBarUI> playerGraphs = new();


    public void DisplayCoinStat()
    {
        foreach(KeyValuePair<ulong, RoundStats> kvp in MatchStatTracker.instance.PlayerStats)
        {
            if(playerGraphs.TryGetValue(kvp.Key, out GraphBarUI graphUI))
            {
                //layerGraphs[kvp.Key].PopulateGraph()
            }
            else
            {
                GraphBarUI newGraph = Instantiate(graphPrefab, barGraphHolder);
                newGraph.SetupGraph("Player");
                playerGraphs.Add(kvp.Key, newGraph);
            }
        }
    }
}
