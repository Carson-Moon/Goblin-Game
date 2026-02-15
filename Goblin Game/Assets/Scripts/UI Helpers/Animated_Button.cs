using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

// Skew button!

public class Animated_Button : Abstract_Animated_UI
{
    [Header("Button Events")]
    [SerializeField] private UnityEvent onButtonPressedEvent;
    public event Action onButtonPressedAction;

    [Header("Button Animation Settings")]
    [SerializeField] protected float rotationDegrees;
    [SerializeField] protected float rotationSeconds;
    [SerializeField] protected float scaleMultiplier;
    [SerializeField] protected float scaleSeconds;
    [SerializeField] protected Color defaultColor;
    [SerializeField] protected Color pressedColor;
    [SerializeField] protected float colorSeconds;


    #region Logic

    public override bool OnRelease(PointerEventData data)
    {
        if (!base.OnRelease(data))
            return false;

        print("Button actually pressed!");
        onButtonPressedEvent?.Invoke();
        onButtonPressedAction?.Invoke();

        return true;
    }

    #endregion

    #region Animations
    protected override void OnHoverAnimation()
    {
        // Rotate our top graphic to askew.
        topGraphic.DORotate(new Vector3(0, 0, rotationDegrees), rotationSeconds, RotateMode.Fast)
            .SetEase(Ease.OutElastic);
    }

    protected override void OffHoverAnimation()
    {
        // Rotate our top graphic back to normal.
        topGraphic.DORotate(new Vector3(0, 0, 0), rotationSeconds, RotateMode.Fast)
            .SetEase(Ease.OutElastic);
    }

    protected override void OnPressAnimation()
    {
        // Scale down our top graphic.
        topGraphic.DOScale(Vector3.one * scaleMultiplier, scaleSeconds)
            .SetEase(Ease.OutElastic);

        // Tween to our pressed color.
        topGraphicImage.DOColor(pressedColor, colorSeconds);
    }

    protected override void OnReleaseAnimation()
    {
        // Scale up our top graphic.
        topGraphic.DOScale(Vector3.one, scaleSeconds)
            .SetEase(Ease.OutElastic);

        // Tween to our default color.
        topGraphicImage.DOColor(defaultColor, colorSeconds);
    }

    protected override void Destroy()
    {
        onButtonPressedEvent = null;
        onButtonPressedAction = null;
    }

    #endregion
}
