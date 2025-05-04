using TMPro;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    [Header("Display Text")]
    public TextMeshPro display;
    public GameManager gManager;


    private void Update() {
        if(display == null)
            return;

        // Format and display the time from the networked timer.
        float totalTime = gManager.GetTimerValue();

        int minutes = Mathf.FloorToInt(totalTime / 60f);
        int seconds = Mathf.FloorToInt(totalTime % 60f);

        display.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
