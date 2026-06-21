using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] CinemachineBasicMultiChannelPerlin cinemachineNoise;

    private Coroutine currentImpulse = null;


    public void PerformImpulseShake()
    {
        if(currentImpulse != null)
        {
            StopCoroutine(currentImpulse);
            currentImpulse = null;
        }

        currentImpulse = StartCoroutine(ImpulseShake());
    }
    
    IEnumerator ImpulseShake()
    {
        cinemachineNoise.AmplitudeGain = 2;
        cinemachineNoise.FrequencyGain = 10;

        yield return new WaitForSeconds(.25f);

        cinemachineNoise.AmplitudeGain = 0;
        cinemachineNoise.FrequencyGain = 0;
    }
}
