using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using System;

public class CeremonyWheel : MonoBehaviour
{
    [Header("Wheel Settings")]
    [SerializeField]  float radius = 5f;
    [SerializeField] float fineTuningAngle = 0f;

    [Header("Label Settings")]
    [SerializeField] float fontSize = 14;
    [SerializeField] float characterSize = 0.1f;
    [SerializeField] bool rotateToFaceOutward = true;

    [Header("Spin Settings")]
    [SerializeField] float spinDuration = 3f;
    [SerializeField] int extraFullRotations = 5;
    [SerializeField] AnimationCurve spinCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private List<GameObject> labelObjects = new();
    private List<(string, string)> categories = new();
    private float currentAngleOffset = 0f;
    private int categoryCount;


    public void SetupWheel(string[] allCategories)
    {
        ClearLabels();

        categoryCount = allCategories.Length;
        float anglePerSegment = 360f / categoryCount;

        for (int i = 0; i < categoryCount; i++)
        {
            float angleDeg = i * anglePerSegment;
            float angleRad = angleDeg * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0f) * radius;

            string categoryTitle = allCategories[i];

            GameObject labelObj = new GameObject("Label_" + categoryTitle);
            labelObj.transform.SetParent(transform, false);
            labelObj.transform.localPosition = pos;

            if (rotateToFaceOutward)
                labelObj.transform.localRotation = Quaternion.Euler(0f, 0f, angleDeg);

            TextMesh tm = labelObj.AddComponent<TextMesh>();
            tm.text = categoryTitle;
            tm.fontSize = (int)fontSize;
            tm.characterSize = characterSize;
            tm.color = UnityEngine.Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
            tm.anchor = TextAnchor.MiddleLeft;
            tm.alignment = TextAlignment.Left;

            labelObjects.Add(labelObj);
            categories.Add((allCategories[i], allCategories[i]));
        }
    }

    public void Spin(string categoryString, Action onSpinComplete)
    {
        int categoryIndex = categories.Select(x => x.Item1).ToList().IndexOf(categoryString);
        if(categoryIndex == -1)
        {
            Debug.Log("Could not spin wheel! Category index is -1!");
            return;
        }

        StartCoroutine(SpinCoroutine(categoryIndex, onSpinComplete));
    }

    IEnumerator SpinCoroutine(int categoryIndex, Action onSpinComplete)
    {
        float anglePerSegment = 360f / categoryCount;

        // Absolute angle that the winner needs to land on
        float winnerAngle = categoryIndex * anglePerSegment + anglePerSegment / 2f;
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

        onSpinComplete?.Invoke();
    }

    void ApplyAngleOffset(float offset)
    {
        int count = categoryCount;
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

    void ClearLabels()
    {
        foreach (GameObject label in labelObjects)
        {
            if (label != null) Destroy(label);
        }
        labelObjects.Clear();
    }
}
