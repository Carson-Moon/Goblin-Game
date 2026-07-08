using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeColorChanger : MonoBehaviour
{
    [SerializeField] GoblinMaterialInstantiater goblinMats;
    [SerializeField] Slider redSlider;
    [SerializeField] Slider greenSlider;
    [SerializeField] Slider blueSlider;
    private Color chosenColor = new Color(0, 1, 0);
    public Color ChosenColor => chosenColor;


    void Start()
    {
        StartCoroutine(SetStartColor());
    }

    IEnumerator SetStartColor()
    {
        yield return new WaitUntil(() => goblinMats.Materials.Count > 0);

        chosenColor = ColorHolder.Color;
        redSlider.value = chosenColor.r;
        greenSlider.value = chosenColor.g;
        blueSlider.value = chosenColor.b;

        foreach(var mat in goblinMats.Materials)
            mat.SetColor("_Hue_Shift", chosenColor);
    }

    public void UpdateChosenColor()
    {
        chosenColor = new Color(redSlider.value, greenSlider.value, blueSlider.value);

        foreach(var mat in goblinMats.Materials)
            mat.SetColor("_Hue_Shift", chosenColor);

        ColorHolder.SetNewColor(chosenColor);
    }
}
