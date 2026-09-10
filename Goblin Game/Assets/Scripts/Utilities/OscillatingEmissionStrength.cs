using System.Collections;
using UnityEngine;

public class OscillatingEmissionStrength : MonoBehaviour
{
    [SerializeField] private Material emissionmaterial;
    [SerializeField] private float oscillationSpeed = 1f;
    [SerializeField] private float minEmissionStrength = 0f;
    [SerializeField] private float maxEmissionStrength = 1f;
    [SerializeField] private bool oscillate = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (emissionmaterial == null)
        {
            Debug.LogError("Emission material is not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (oscillate)
        {
            StartCoroutine(OscillateEmissionStrength());
        }
    }

    // Coroutine to oscillate the emission strength
    IEnumerator OscillateEmissionStrength()
    {
        float time = 0f;
        while (oscillate)
        {
            time += Time.deltaTime;
            float emissionStrength = Mathf.PingPong(time, maxEmissionStrength - minEmissionStrength) + minEmissionStrength; // Oscillates between minEmissionStrength and maxEmissionStrength
            emissionmaterial.SetFloat("_emissionStrength", emissionStrength);
            yield return null;
        }
    }
}
