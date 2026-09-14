using System.Collections;
using TMPro;
using UnityEngine;

public class ObjectiveWinnerDisplay : MonoBehaviour
{
    [SerializeField] CanvasGroup display;
    [SerializeField] TextMeshProUGUI winner;
    [SerializeField] float displayTime;


    public void DisplayWinner(ulong winnerID)
    {
        StartCoroutine(DisplayWinnerForTime(winnerID));
    }

    IEnumerator DisplayWinnerForTime(ulong winnerID)
    {
        winner.text = winnerID.GetUsername();
        display.alpha = 1;

        yield return new WaitForSeconds(displayTime);

        display.alpha = 0;
    }
}
