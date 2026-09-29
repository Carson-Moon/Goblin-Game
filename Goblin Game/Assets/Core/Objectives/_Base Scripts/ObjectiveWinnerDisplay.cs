using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class ObjectiveWinnerDisplay : MonoBehaviour
{
    public static ObjectiveWinnerDisplay Instance {get; private set;}

    [SerializeField] CanvasGroup display;
    [SerializeField] TextMeshProUGUI winner;
    [SerializeField] float displayTime;


    void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void DisplayWinners(List<ulong> winnerIDs)
    {
        StartCoroutine(DisplayWinnersForTime(winnerIDs));
    }

    IEnumerator DisplayWinnersForTime(List<ulong> winnerIDs)
    {
        StringBuilder winners = new();
        foreach(var winner in winnerIDs)
            winners.Append($"{winner.GetUsername()}<br>");
        winner.text = winners.ToString();
        display.alpha = 1;

        yield return new WaitForSeconds(displayTime);

        display.alpha = 0;
    }
}
