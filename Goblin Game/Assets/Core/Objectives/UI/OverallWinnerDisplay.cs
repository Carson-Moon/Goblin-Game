using System.Collections.Generic;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class OverallWinnerDisplay : MonoBehaviour
{
    public static OverallWinnerDisplay Instance {get; private set;}

    [SerializeField] CanvasGroup display;
    [SerializeField] TextMeshProUGUI winnerDisplay;


    void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        display.alpha = 0;
    }

    public void DisplayOverallWinners(List<ulong> winners)
    {
        StartCoroutine(DisplayDelay(winners));
    }

    IEnumerator DisplayDelay(List<ulong> winners)
    {
        yield return new WaitForSeconds(8);

        StringBuilder winnersText = new();
        foreach(var winner in winners)
            winnersText.Append($"{winner.GetUsername()}<br>");
        winnerDisplay.text = winnersText.ToString();

        display.alpha = 1;
    }


}
