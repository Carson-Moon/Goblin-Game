using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

// Flip toggle!

public class Animated_Toggle : Abstract_Animated_UI
{
    [Header("Toggle Settings")]
    [SerializeField] bool isOn = false;
    private Quaternion baseHolderRotation;

    [Header("Toggle Events")]
    [SerializeField] UnityEvent onToggleOn;
    [SerializeField] UnityEvent onToggleOff;

    [Header("Toggle Animation Settings")]
    [SerializeField] RectTransform topGraphicHolder;
    [SerializeField] float rotationDegrees;
    [SerializeField] float rotationSeconds;
    [SerializeField] float skewDegrees;
    [SerializeField] float skewSeconds;
    [SerializeField] float scaleMultiplier;
    [SerializeField] float scaleSeconds;
    [SerializeField] protected Color onColor;
    [SerializeField] protected Color onColorPressed;
    [SerializeField] protected Color offColor;
    [SerializeField] protected Color offColorPressed;
    [SerializeField] protected float colorSeconds;
    [SerializeField] GameObject onText;
    [SerializeField] GameObject offText;


    #region Logic

    protected override void Start()
    {
        base.Start();

        baseHolderRotation = topGraphicHolder.localRotation;
    }

    public override bool OnRelease(PointerEventData data)
    {
        if (!base.OnRelease(data))
            return false;

        print("Toggle actually flipped!");
        FlipToggle();

        return true;
    }

    private void FlipToggle()
    {
        isOn = !isOn;

        if (isOn)
        {
            onToggleOn.Invoke();
            ToggleOnGraphics();
        }
        else
        {
            onToggleOff.Invoke();
            ToggleOffGraphics();
        }       
    }

    private void ToggleOnGraphics()
    {
        // Flip our graphic back to normal.
        topGraphic.DOLocalRotate(new Vector3(0, 0, 0), rotationSeconds, RotateMode.Fast)
            .SetEase(Ease.OutElastic);

        // Tween to our default color.
        topGraphicImage.DOColor(onColor, colorSeconds);

        onText.SetActive(true);
        offText.SetActive(false);
    }

    private void ToggleOffGraphics()
    {
        // Flip our graphic to backwards.
        topGraphic.DOLocalRotate(new Vector3(rotationDegrees, 0, 0), rotationSeconds, RotateMode.Fast)
            .SetEase(Ease.OutElastic);

        // Tween to our on color.
        topGraphicImage.DOColor(offColor, colorSeconds);

        onText.SetActive(false);
        offText.SetActive(true);
    }

    #endregion

    #region Animations

    protected override void OnHoverAnimation()
    {
        // Rotate our top graphic to askew.
        topGraphicHolder.DORotate(new Vector3(0, 0, skewDegrees), rotationSeconds, RotateMode.Fast)
            .SetEase(Ease.OutElastic);
    }

    protected override void OffHoverAnimation()
    {
        // Rotate our top graphic to normal.

        topGraphicHolder.DORotate(Vector3.zero, rotationSeconds, RotateMode.Fast)
            .SetEase(Ease.OutElastic);

        print("OFF HOVER");
    }

    protected override void OnPressAnimation()
    {
        // Scale down our top graphic.
        topGraphic.DOScale(Vector3.one * scaleMultiplier, scaleSeconds)
            .SetEase(Ease.OutElastic);

        if (isOn)
        {
            // Tween to our on pressed color.
            topGraphicImage.DOColor(onColorPressed, colorSeconds);
        }
        else
        {
            // Tween to our off pressed color.
            topGraphicImage.DOColor(offColorPressed, colorSeconds);
        }
    }

    protected override void OnReleaseAnimation()
    {
        // Scale up our top graphic.
        topGraphic.DOScale(Vector3.one, scaleSeconds)
            .SetEase(Ease.OutElastic);

        if (isOn)
        {
            // Tween to our on released color.
            topGraphicImage.DOColor(onColor, colorSeconds);
        }
        else
        {
            // Tween to our off released color.
            topGraphicImage.DOColor(offColor, colorSeconds);
        }
    }

    protected override void Destroy()
    {
        onToggleOn = null;
        onToggleOff = null;
    }

    #endregion
}
