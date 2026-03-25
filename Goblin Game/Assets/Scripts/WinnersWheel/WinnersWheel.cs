using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WinnersWheel : MonoBehaviour
{
    [Header("Debug Settings")]
    public bool isDebug = false;
    public bool reinitializeCategories = false;
    public bool spinWheelButton = false;
    public float fineTuningAngle = 0f;

    [Header("Categories")]
    public List<string> categories = new List<string>();

    [Header("Wheel Settings")]
    public float radius = 5f;

    [Header("Label Settings")]
    public float fontSize = 14;
    public float characterSize = 0.1f;
    public Color labelColor = Color.white;
    public bool rotateToFaceOutward = true;

    [Header("Spin Settings")]
    public float spinDuration = 3f;
    public int extraFullRotations = 5;
    public AnimationCurve spinCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    public List<GameObject> labelObjects = new List<GameObject>();
    private int winnerIndex = -1;
    private float currentAngleOffset = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateWheel();
    }

    void Update()
    {
        if (isDebug && reinitializeCategories)
        {
            reinitializeCategories = false;
            GenerateWheel();
        }
        if (isDebug && spinWheelButton)
        {
            spinWheelButton = false;
            Spin();
        }
    }

    // take categories and space them equidistant from one another, create text objects.
    public void GenerateWheel()
    {
        ClearLabels();

        int count = categories.Count;
        float anglePerSegment = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angleDeg = i * anglePerSegment;
            float angleRad = angleDeg * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0f) * radius;

            GameObject labelObj = new GameObject("Label_" + categories[i]);
            labelObj.transform.SetParent(transform, false);
            labelObj.transform.localPosition = pos;

            if (rotateToFaceOutward)
            {
                labelObj.transform.localRotation = Quaternion.Euler(0f, 0f, angleDeg);
            }

            TextMesh tm = labelObj.AddComponent<TextMesh>();
            tm.text = categories[i];
            tm.fontSize = (int)fontSize;
            tm.characterSize = characterSize;
            tm.color = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
            tm.anchor = TextAnchor.MiddleLeft;
            tm.alignment = TextAlignment.Left;

            labelObjects.Add(labelObj);
        }
    }

    public void Spin()
    {
        if (categories.Count == 0) return;

        // determine the winner immediately before any animation
        winnerIndex = Random.Range(0, categories.Count);
        StartCoroutine(SpinCoroutine(winnerIndex));
    }

    IEnumerator SpinCoroutine(int winner)
    {
        float anglePerSegment = 360f / categories.Count;

        // Absolute angle that the winner needs to land on
        float winnerAngle = winner * anglePerSegment + anglePerSegment / 2f;
        float targetAbsolute = 90f - winnerAngle + fineTuningAngle;

        // normalize to 0-360 
        targetAbsolute = ((targetAbsolute % 360f) + 360f) % 360f;

        // find the shortest positive delta from current position to the target, then pad with additional rotations.
        float delta = targetAbsolute - (currentAngleOffset % 360f);
        if (delta < 0f) delta += 360f;
        delta += extraFullRotations * 360f;

        float startOffset = currentAngleOffset;
        float elapsed = 0f;

        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / spinDuration);

            // sample the animation for easing
            float curvedT = spinCurve.Evaluate(t);
            currentAngleOffset = Mathf.LerpUnclamped(startOffset, startOffset + delta, curvedT);

            ApplyAngleOffset(currentAngleOffset);
            yield return null;
        }

        // snap it to the final position at the end
        currentAngleOffset = startOffset + delta;
        ApplyAngleOffset(currentAngleOffset);

        Debug.Log("Winner: " + categories[winner]);
        OnSpinComplete(winner);
    }

    void ApplyAngleOffset(float offset)
    {
        int count = categories.Count;
        float anglePerSegment = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angleDeg = i * anglePerSegment + offset;
            float angleRad = angleDeg * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0f) * radius;
            labelObjects[i].transform.localPosition = pos;

            if (rotateToFaceOutward)
            {
                labelObjects[i].transform.localRotation = Quaternion.Euler(0f, 0f, angleDeg);
            }
        }
    }

    void OnSpinComplete(int winner)
    {
        // trigger any necessary events here
        Debug.Log("Spin Complete: Winning Category: " + categories[winner]);
    }

    void ClearLabels()
    {
        foreach (GameObject label in labelObjects)
        {
            if (label != null) Destroy(label);
        }
        labelObjects.Clear();
    }
}
