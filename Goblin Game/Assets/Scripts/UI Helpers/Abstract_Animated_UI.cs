using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

// Responsible for holding what we need to animate a UI button/toggle.

[RequireComponent(typeof(EventTrigger))]
public abstract class Abstract_Animated_UI : MonoBehaviour
{
    [Header("Runtime Settings")]
    [SerializeField] protected bool isPressing;
    protected EventTrigger eventTrigger;

    [Header("Top Graphic Settings")]
    [SerializeField] protected RectTransform topGraphic;
    [SerializeField] protected Image topGraphicImage;


    #region Setup

    protected virtual void Start()
    {
        SetupEventTrigger();
    }

    protected virtual void SetupEventTrigger()
    {
        eventTrigger = GetComponent<EventTrigger>();

        SetupEventTriggerEntry(EventTriggerType.PointerEnter, new Action<PointerEventData>[] {OnHover});
        SetupEventTriggerEntry(EventTriggerType.PointerExit, new Action<PointerEventData>[] {OffHover});
        SetupEventTriggerEntry(EventTriggerType.PointerDown, new Action<PointerEventData>[] {OnPress});
        SetupEventTriggerEntry(EventTriggerType.PointerUp, new Action<PointerEventData>[] {OnReleaseWrapper});
    }

    protected void SetupEventTriggerEntry(EventTriggerType triggerType, Action<PointerEventData>[] functions)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = triggerType
        };

        foreach (Action<PointerEventData> function in functions)
        {
            entry.callback.AddListener((eventData) => { function((PointerEventData)eventData); });
        }
        
        eventTrigger.triggers.Add(entry);
    }

    #endregion

    #region Logic

    public void OnHover(PointerEventData data)
    {
        OnHoverAnimation();
    }

    public void OffHover(PointerEventData data)
    {
        if (isPressing)
        {
            isPressing = false;

            OnReleaseAnimation();
        }

        OffHoverAnimation();
    }

    public void OnPress(PointerEventData data)
    {
        isPressing = true;

        OnPressAnimation();
    }

    public void OnReleaseWrapper(PointerEventData data)
    {
        OnRelease(data);
    }

    public virtual bool OnRelease(PointerEventData data)
    {
        if (!isPressing)
            return false;

        isPressing = false;

        OnReleaseAnimation();

        return true;
    }

    #endregion

    #region Animations

    protected abstract void OnHoverAnimation();

    protected abstract void OffHoverAnimation();

    protected abstract void OnPressAnimation();

    protected abstract void OnReleaseAnimation();

    #endregion

    protected abstract void Destroy();

    private void OnDestroy()
    {
        Destroy();
    }
}
