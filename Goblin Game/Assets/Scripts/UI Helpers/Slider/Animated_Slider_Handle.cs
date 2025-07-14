using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

// Extra logic for our slider handle.

public class Animated_Slider_Handle : Animated_Button
{
    // Runtime
    Animated_Slider_Controller sliderController;
    private bool lastDirection = true;


    void Update()
    {
        if (isPressing)
        {         

        }
    }

    protected override void SetupEventTrigger()
    {
        eventTrigger = GetComponent<EventTrigger>();
        sliderController = GetComponentInParent<Animated_Slider_Controller>();

        SetupEventTriggerEntry(EventTriggerType.PointerEnter, new Action<PointerEventData>[] { OnHover });
        SetupEventTriggerEntry(EventTriggerType.PointerExit, new Action<PointerEventData>[] { OffHover });
        SetupEventTriggerEntry(EventTriggerType.PointerDown, new Action<PointerEventData>[] { OnPress, sliderController.EnableHandleSliding });
        SetupEventTriggerEntry(EventTriggerType.PointerUp, new Action<PointerEventData>[] { OnReleaseWrapper, sliderController.DisableHandleSliding });
    }

    #region Animation

    protected void OnSlidingAnimation()
    {
        topGraphic.DORotate(new Vector3(0, 0, rotationDegrees), rotationSeconds, RotateMode.Fast)
            .SetEase(Ease.OutElastic);
    }

    protected override void OnHoverAnimation()
    {
        base.OnHoverAnimation();
    }

    protected override void OffHoverAnimation()
    {
        base.OffHoverAnimation();
    }

    #endregion
}
