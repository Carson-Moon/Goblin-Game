using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MovementSetting : MonoBehaviour
{
    public Slider slider;
    [SerializeField] float minValue;
    [SerializeField] float maxValue;
    [SerializeField] TextMeshProUGUI display;
    [SerializeField] TextMeshProUGUI title;


    void Awake()
    {
        title.text = gameObject.name;
        slider.minValue = minValue;
        slider.maxValue = maxValue;
    }

    void Update()
    {
        display.text = slider.value.ToString("F1");
    }
}
