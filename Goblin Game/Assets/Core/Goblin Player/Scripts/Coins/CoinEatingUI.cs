using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoinEatingUI : MonoBehaviour
{
    [SerializeField] Slider progress;
    [SerializeField] CanvasGroup canvasGroup;

    private Coroutine eatingCoroutine = null;
    private bool isEating = false;


    void Awake()
    {
        canvasGroup.TurnOff();
    }

    public void StartEatingUI(float eatingLength, Action onComplete)
    {
        isEating = true;

        canvasGroup.TurnOn();

        eatingCoroutine = StartCoroutine(RunEatingUI(eatingLength, onComplete));
    }

    IEnumerator RunEatingUI(float eatingLength, Action onComplete)
    {
        progress.value = 0;

        float time = 0;

        while(time < eatingLength)
        {
            progress.value = Mathf.Lerp(0, 1, time / eatingLength);
            time += Time.deltaTime;
            yield return null;
        }

        progress.value = 1;

        onComplete?.Invoke();

        if(isEating)
            eatingCoroutine = StartCoroutine(RunEatingUI(eatingLength, onComplete));
    }

    public void StopEatingUI()
    {
        canvasGroup.TurnOff();

        if(eatingCoroutine != null)
        {
            StopCoroutine(eatingCoroutine);
            eatingCoroutine = null;
        }
    }
}
