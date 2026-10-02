using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public class ObjectiveWinnerDisplay : MonoBehaviour
{
    public static ObjectiveWinnerDisplay Instance {get; private set;}

    [SerializeField] CanvasGroup display;
    [SerializeField] TextMeshProUGUI winner;
    [SerializeField] PlayerPointAnnouncementUI pointPrefab;
    [SerializeField] RectTransform pointHolder;
    private List<PlayerPointAnnouncementUI> points = new();
    [SerializeField] float displayTime;


    void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void DisplayWinners(List<ulong> winnerIDs, Dictionary<ulong, int> playerPoints)
    {
        DisplayCurrentWinners(winnerIDs);
        DisplayAllPlayerPoints(playerPoints);
        StartCoroutine(DisplayWinnersForTime());
    }

    private void DisplayCurrentWinners(List<ulong> winnerIDs)
    {
        StringBuilder winners = new();
        foreach(var winner in winnerIDs)
            winners.Append($"{winner.GetUsername()}<br>");
        winner.text = winners.ToString();
    }

    private void DisplayAllPlayerPoints(Dictionary<ulong, int> playerPoints)
    {
        foreach(var point in points)
            Destroy(point.gameObject);
        points.Clear();

        var orderedPoints = playerPoints.OrderByDescending(x => x.Value).ThenBy(x => x.Key);
        foreach(var player in orderedPoints)
        {
            PlayerPointAnnouncementUI newPoints = Instantiate(pointPrefab, pointHolder);
            newPoints.Initialize(player.Key.GetUsername(), player.Value);
            points.Add(newPoints);
        }
    }

    IEnumerator DisplayWinnersForTime()
    {
        display.alpha = 1;

        yield return new WaitForSeconds(displayTime);

        display.alpha = 0;
    }
}
