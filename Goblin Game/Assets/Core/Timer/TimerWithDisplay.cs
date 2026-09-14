using TMPro;
using UnityEngine;

public class TimerWithDisplay : Timer
{
    [SerializeField] TextMeshProUGUI timerDisplay;


    void Update()
    {
        if(isRunning)
            timerDisplay.text = value.ToString("F0");
        else
            timerDisplay.text = string.Empty;
    }
}
