using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;

public class WinnersWheel : MonoBehaviour
{
    [Header("Debug Settings")]
    public bool isDebug = false;
    public bool reinitializeCategories = false;
    public bool spinWheelButton = false;
    public float fineTuningAngle = 0f;

    [Header("Categories")]
    public List<IntStat> intCategories = new();
    public List<FloatStat> floatCategories = new();
    private int categoryCount;
    private List<string> categories = new List<string>();
    private List<string> chosenCategories = new List<string>();

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
    private float currentAngleOffset = 0f;


    // void Start()
    // {
    //     GenerateWheel(intCategories, floatCategories);
    // }

    void Update()
    {
        if (isDebug && reinitializeCategories)
        {
            reinitializeCategories = false;
            GenerateWheel(intCategories.Select(x => (int)x).ToArray(), floatCategories.Select(x => (int)x).ToArray());
        }
        if (isDebug && spinWheelButton)
        {
            spinWheelButton = false;
            int category = ChooseCategory();
            Spin(category);
        }
    }

    // take categories and space them equidistant from one another, create text objects.
    public void GenerateWheel(int[] intStats, int[] floatStats)
    {
        ClearLabels();

        intCategories = intStats.Select(x => (IntStat)x).ToList();
        floatCategories = floatStats.Select(x => (FloatStat)x).ToList();

        int count = intCategories.Count + floatCategories.Count;
        float anglePerSegment = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angleDeg = i * anglePerSegment;
            float angleRad = angleDeg * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0f) * radius;

            string categoryTitle = i < intStats.Length ? intCategories[i].ToString() : floatCategories[i - intStats.Length].ToString();

            GameObject labelObj = new GameObject("Label_" + categoryTitle);
            labelObj.transform.SetParent(transform, false);
            labelObj.transform.localPosition = pos;

            if (rotateToFaceOutward)
            {
                labelObj.transform.localRotation = Quaternion.Euler(0f, 0f, angleDeg);
            }

            TextMesh tm = labelObj.AddComponent<TextMesh>();
            tm.text = categoryTitle;
            tm.fontSize = (int)fontSize;
            tm.characterSize = characterSize;
            tm.color = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
            tm.anchor = TextAnchor.MiddleLeft;
            tm.alignment = TextAlignment.Left;

            labelObjects.Add(labelObj);
            categories.Add(categoryTitle);
        }

        categoryCount = categories.Count;
    }

    public int ChooseCategory()
    {
        if (categories.Count == 0) return -1;

        return Random.Range(0, categories.Count);
    }

    public void Spin(int index)
    {
        StartCoroutine(SpinCoroutine(index));
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

        DetermineWinner(categories[winner]);
        OnSpinComplete(categories[winner]);
    }

    private void DetermineWinner(string category)
    {
        if(intCategories.Select(x => x.ToString()).Contains(category))
            Debug.Log($"Winner is an Int Stat: {category}");
        else
            Debug.Log($"Winner is a Float Stat: {category}");
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

    void OnSpinComplete(string category)
    {
        // trigger any necessary events here
        chosenCategories.Add(category);
        categories.Remove(category);
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
