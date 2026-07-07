using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeColorChanger : MonoBehaviour
{
    [SerializeField] GoblinMaterialInstantiater goblinMats;
    [SerializeField] Slider redSlider;
    [SerializeField] Slider greenSlider;
    [SerializeField] Slider blueSlider;
    private Color chosenColor = new Color(1, 1, 1);
    public Color ChosenColor => chosenColor;


    void Start()
    {
        StartCoroutine(SetStartColor());
    }

    IEnumerator SetStartColor()
    {
        yield return new WaitUntil(() => goblinMats.Materials.Count > 0);

        foreach(var mat in goblinMats.Materials)
            mat.SetColor("_Hue_Shift", chosenColor);

        Debug.Log("Set start color.");
    }

    public void UpdateChosenColor()
    {
        chosenColor = new Color(redSlider.value, greenSlider.value, blueSlider.value);

        foreach(var mat in goblinMats.Materials)
            mat.SetColor("_Hue_Shift", chosenColor);

        ColorHolder.SetNewColor(chosenColor);
    }
}
