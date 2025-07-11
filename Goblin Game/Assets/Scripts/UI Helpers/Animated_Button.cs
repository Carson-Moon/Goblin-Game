using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;

// Responsible for holding what we need to animate a UI button.
    // Skew button!

public class Animated_Button : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] bool isPressing;

    [Header("Top Graphic Settings")]
    [SerializeField] RectTransform topGraphic;
    [SerializeField] Image topGraphicImage;
    [SerializeField] float rotationDegrees;
    [SerializeField] float rotationSeconds;
    [SerializeField] float scaleMultiplier;
    [SerializeField] float scaleSeconds;
    [SerializeField] Color defaultColor;
    [SerializeField] Color pressedColor;
    [SerializeField] float colorSeconds;

    [Header("Button Events")]
    [SerializeField] UnityEvent onButtonPressed;


    public void OnHover()
    {
        OnHoverAnimation();
    }

    public void OffHover()
    {
        if (isPressing)
        {
            isPressing = false;

            OnReleaseAnimation();
        }

        OffHoverAnimation();
    }

    public void OnPress()
    {
        isPressing = true;

        OnPressAnimation();
    }

    public void OnRelease()
    {
        if (!isPressing)
            return;

        isPressing = false;
        onButtonPressed.Invoke();
        print("Button actually pressed!");

        OnReleaseAnimation();
    }

    #region Animations
    
    private void OnHoverAnimation()
    {
        // Rotate our top graphic to askew.
        topGraphic.DORotate(new Vector3(0, 0, rotationDegrees), rotationSeconds, RotateMode.Fast)
            .SetEase(Ease.OutElastic);
    }

    private void OffHoverAnimation()
    {
        // Rotate our top graphic back to normal.
        topGraphic.DORotate(new Vector3(0, 0, 0), rotationSeconds, RotateMode.Fast)
            .SetEase(Ease.OutElastic);
    }

    private void OnPressAnimation()
    {
        // Scale down our top graphic.
        topGraphic.DOScale(Vector3.one * scaleMultiplier, scaleSeconds)
            .SetEase(Ease.OutElastic);

        // Tween to our pressed color.
        topGraphicImage.DOColor(pressedColor, colorSeconds);
    }

    private void OnReleaseAnimation()
    {
        // Scale up our top graphic.
        topGraphic.DOScale(Vector3.one, scaleSeconds)
            .SetEase(Ease.OutElastic);

        // Tween to our default color.
        topGraphicImage.DOColor(defaultColor, colorSeconds);
    }

    #endregion
}
