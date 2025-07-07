using UnityEngine;
using DG.Tweening;
using System.Collections;
using TMPro;
using System;

public class CountdownCanvas : MonoBehaviour
{
    [SerializeField] static CanvasGroup fadingCanvas;
    [SerializeField] TextMeshProUGUI countdownText;
    public Action onCountdownEnd;


    void Awake()
    {
        fadingCanvas = GetComponent<CanvasGroup>();
    }

    public void StartCountdown()
    {
        StartCoroutine(PerformCountdown());
    }

    IEnumerator PerformCountdown()
    {
        // Wait one second.
        yield return new WaitForSeconds(1);

        // Enable our countdown text at three.
        fadingCanvas.alpha = 1;
        countdownText.text = "3";
        StartFade(0, 1);

        // Wait one second.
        yield return new WaitForSeconds(1);

        // 2.
        countdownText.text = "2";
        fadingCanvas.alpha = 1;
        StartFade(0, 1);

        // Wait one second.
        yield return new WaitForSeconds(1);

        // 1.
        countdownText.text = "1";
        fadingCanvas.alpha = 1;
        StartFade(0, 1);

        // Wait one second.
        yield return new WaitForSeconds(1);

        // We are done!
        onCountdownEnd.Invoke();
    }

    // Fade this canvas.
    public static void StartFade(float alpha, float fadeTime)
    {
        fadingCanvas.DOFade(alpha, fadeTime);
    }
}
