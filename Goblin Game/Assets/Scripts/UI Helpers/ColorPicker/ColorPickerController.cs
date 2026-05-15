using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorPickerController : MonoBehaviour
{
    public float currentHue, currentSat, currentVal;

    [SerializeField] private RawImage hueImage, satValImage, outputImage;

    [SerializeField] private Slider hueSlider;

    [SerializeField] private TMP_InputField hexInputField;
    
    private Texture2D hueTexture, svTexture, outputTexture;

    [SerializeField] Color currentColor;

    [Header("Materials")]
    [SerializeField] private Renderer characterRenderer;
    [SerializeField] private int faceMaterialIndex = 0;
    [SerializeField] private int bodyMaterialIndex = 1;

    private Material faceMaterial, bodyMaterial;    

    private void Start()
    {
        // .materials (plural) auto-instances ALL materials on the renderer
        Material[] mats = characterRenderer.materials;
        faceMaterial = mats[faceMaterialIndex];
        bodyMaterial = mats[bodyMaterialIndex];

        CreateHueImage();
        CreateSVImage();
        CreateOutputImage();

        UpdateOutputImage();
    }

    private void CreateHueImage()
    {
        hueTexture = new Texture2D(1, 16);
        hueTexture.wrapMode = TextureWrapMode.Clamp;
        hueTexture.name = "HueTexture";

        for (int i = 0; i < hueTexture.height; i++)
        {
            hueTexture.SetPixel(0, i, Color.HSVToRGB((float)i / hueTexture.height, 1, 1.00f));
        }

        hueTexture.Apply();
        currentHue = 0;

        hueImage.texture = hueTexture;
    }

    private void CreateSVImage()
    {
        svTexture = new Texture2D(16, 16);
        svTexture.wrapMode = TextureWrapMode.Clamp;
        svTexture.name = "SatValTexture";

        for(int y = 0; y < svTexture.height; y++)
        {
            for(int x = 0; x < svTexture.width; x++)
            {
                svTexture.SetPixel(x, y, Color.HSVToRGB(
                    currentHue, 
                    (float)x / svTexture.width, 
                    (float)y / svTexture.height));
            }
        }

        svTexture.Apply();
        currentSat = 0;
        currentVal = 0;

        satValImage.texture = svTexture;
    }

    private void CreateOutputImage()
    {
        outputTexture = new Texture2D(1, 16);
        outputTexture.wrapMode = TextureWrapMode.Clamp;
        outputTexture.name = "OutputTexture";

        Color currentColor = Color.HSVToRGB(currentHue, currentSat, currentVal);

        for (int i = 0; i < outputTexture.height; i++)
        {
            outputTexture.SetPixel(0, i, currentColor);
        }

        outputTexture.Apply();

        outputImage.texture = outputTexture;
    }

    private void UpdateOutputImage()
    {
        Color currentColor = Color.HSVToRGB(currentHue, currentSat, currentVal);

        for (int i = 0; i < outputTexture.height; i++)
        {
            outputTexture.SetPixel(0, i, currentColor);
        }

        outputTexture.Apply();

        outputImage.material.SetFloat("_HueShift", currentHue);
        faceMaterial.SetFloat("_HueShift", currentHue);
        bodyMaterial.SetFloat("_HueShift", currentHue);
    }

    public void SetSV(float S, float V)
    {
        currentSat = S;
        currentVal = V;

        UpdateOutputImage();
    }

    public void UpdateSVImage()
    {
        currentHue = hueSlider.value;

        for(int y = 0; y < svTexture.height; y++)
        {
            for(int x = 0; x < svTexture.width; x++)
            {
                svTexture.SetPixel(x, y, Color.HSVToRGB(
                    currentHue, 
                    (float)x / svTexture.width,
                    (float)y / svTexture.height
                ));
            }
        }

        svTexture.Apply();

        UpdateOutputImage();
    }
}
