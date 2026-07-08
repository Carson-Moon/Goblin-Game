using UnityEngine;
using UnityEngine.UI;

public class ColorSetter : MonoBehaviour
{
    [SerializeField] GoblinMaterialInstantiater goblinMats;
    [SerializeField] Slider redSlider;
    [SerializeField] Slider greenSlider;
    [SerializeField] Slider blueSlider;


    void Start()
    {
        SetExistingColor();
        redSlider.onValueChanged.AddListener(OnValueChanged);
        greenSlider.onValueChanged.AddListener(OnValueChanged);
        blueSlider.onValueChanged.AddListener(OnValueChanged);
    }

    private void SetExistingColor()
    {
        goblinMats.Initialize();
        Color existingColor = ColorHolder.FetchExistingColor();
        goblinMats.SetMaterialHueShift(existingColor);
        redSlider.value = existingColor.r;
        greenSlider.value = existingColor.g;
        blueSlider.value = existingColor.b;
    }

    public void OnValueChanged(float value)
    {
        Color newColor = new Color(redSlider.value, greenSlider.value, blueSlider.value);
        ColorHolder.SetNewColor(newColor);
        goblinMats.SetMaterialHueShift(newColor);
    }
}
