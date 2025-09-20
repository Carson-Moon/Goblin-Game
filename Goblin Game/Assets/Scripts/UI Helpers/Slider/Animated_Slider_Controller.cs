using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Animated slider that uses Animated_Buttons for logic control.

public class Animated_Slider_Controller : MonoBehaviour
{
    [Header("Slider Value Settings")]
    [SerializeField] float currentSliderPercentage;
    [SerializeField] float currentSliderValue;
    [SerializeField] float minValue;
    [SerializeField] float maxValue;
    [SerializeField] float sliderRange;
    [SerializeField] float valueRange;

    [Header("Slider Settings")]
    [SerializeField] bool isSliding;
    [SerializeField] RectTransform handleTransform;
    [SerializeField] RectTransform leftEdge;
    [SerializeField] RectTransform rightEdge;
    [SerializeField] private float xPosition;

    [Header("UI Displays")]
    [SerializeField] TextMeshProUGUI sliderValueText;


    void Start()
    {
        SetupSliderValues();
    }

    void Update()
    {
        DetermineXMovement();
    }

    void LateUpdate()
    {
        if (isSliding)
        {
            ApplyHandleMovement();
            DetermineSliderValue();

            sliderValueText.text = currentSliderValue.ToString("F2");
        }
            
    }

    // Determine how far we should slide.
    public void DetermineXMovement()
    {
        // Grab our mouse delta!
        float xMousePos = UI_Input.GetMousePosition().x;
        //print(UI_Input.GetMouseDelta().x);

        xPosition = xMousePos;
    }

    // Apply our xMovement.
    private void ApplyHandleMovement()
    {
        // Grab our projected position.
        Vector3 projectedPosition = new Vector3(xPosition, handleTransform.position.y, handleTransform.position.z);

        // Does this projection break out of our bounds.
        if (projectedPosition.x < leftEdge.position.x)
        {
            projectedPosition = new Vector3(leftEdge.position.x, projectedPosition.y, projectedPosition.z);
        }
        else if (projectedPosition.x > rightEdge.position.x)
        {
            projectedPosition = new Vector3(rightEdge.position.x, projectedPosition.y, projectedPosition.z);
        }

        handleTransform.position = projectedPosition;
    }

    // Setup slider values.
    private void SetupSliderValues()
    {
        sliderRange = rightEdge.position.x - leftEdge.position.x;
        valueRange = maxValue - minValue;
    }

    // Determine and set our slider value.
    private void DetermineSliderValue()
    {
        currentSliderPercentage = (handleTransform.position.x - leftEdge.position.x) / sliderRange;
        currentSliderValue = minValue + (valueRange * currentSliderPercentage);
    }

    // Enable handle sliding.
    public void EnableHandleSliding(PointerEventData data)
    {
        isSliding = true;
    }

    // Disable handle sliding.
    public void DisableHandleSliding(PointerEventData data)
    {
        isSliding = false;
    }
}
