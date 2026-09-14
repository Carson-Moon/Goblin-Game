using System;
using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    protected float value = 0;
    protected bool isRunning = false;
    private Action OnComplete;

    private Coroutine timer = null;


    public void StartTimer(float timerLength, Action onComplete)
    {
        if(timer != null)
        {
            Debug.Log("Timer was already started and not ended properly. Returning.");
            return;
        }

        value = timerLength;
        OnComplete += onComplete;

        isRunning = true;
        StartCoroutine(PerformTimer());
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void UnpauseTimer()
    {
        isRunning = true;
    }

    public void EndTimer(bool performOnComplete)
    {
        isRunning = false;

        if(timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }

        if(performOnComplete)
            OnComplete?.Invoke();

        OnComplete = null;
    }


    IEnumerator PerformTimer()
    {
        while(value > 0)
        {
            if(isRunning)
            {
                value -= Time.deltaTime;
            }

            yield return null;
        }
        
        EndTimer(true);
    }

    void OnDisable()
    {
        OnComplete = null;
    }
}
