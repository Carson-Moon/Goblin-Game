using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] CinemachineCamera cineCam;
    [SerializeField] float defaultValue;
    [SerializeField] float zoomInValue;
    [SerializeField] AnimationCurve zoomEase;

    private Coroutine zoomCoroutine;


    public void StartZoomIn(float durationToMax)
    {
        if(zoomCoroutine != null)
        {
            StopCoroutine(zoomCoroutine);
            zoomCoroutine = null;
        }

        zoomCoroutine = StartCoroutine(ChangeFieldOfViewOverTime(durationToMax, zoomInValue));
    }

    public void StopZoomIn()
    {
        if(zoomCoroutine != null)
        {
            StopCoroutine(zoomCoroutine);
            zoomCoroutine = null;
        }

        zoomCoroutine = StartCoroutine(ChangeFieldOfViewOverTime(0.5f, defaultValue));
    }

    IEnumerator ChangeFieldOfViewOverTime(float durationToMax, float endFOV)
    {
        float time = 0;
        float startFOV = cineCam.Lens.FieldOfView;
        float currentValue = startFOV;

        while(time < durationToMax)
        {
            currentValue = Mathf.Lerp(startFOV, endFOV, zoomEase.Evaluate(time / durationToMax));
            cineCam.Lens.FieldOfView = currentValue;

            time += Time.deltaTime;
            yield return null;
        }
        cineCam.Lens.FieldOfView = endFOV;
    }
}
