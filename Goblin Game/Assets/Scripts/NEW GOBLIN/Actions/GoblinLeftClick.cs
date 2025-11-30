using DG.Tweening;
using UnityEngine;

// Goblin left hand actions:
// Stab.
// Hold to throw if we have a jar in the other hand.

public class GoblinLeftClick : AbstractButtonInputContainer
{
    [SerializeField] StabAction stabAction;
    [SerializeField] GrabAction grabAction;
    [SerializeField] ThrowAction throwAction;

    [SerializeField] bool isHolding = false;
    [SerializeField] bool pastHoldingThreshold = false;
    [SerializeField] float holdThreshold;
    [SerializeField] float currentHoldThreshold;

    protected override void OnPressedAction()
    {
        if (grabAction.CurrentPickup == null)
            stabAction.AttemptStab();
        else
        {
            currentHoldThreshold = holdThreshold;
            isHolding = true;
            pastHoldingThreshold = false;
        }
    }

    void Update()
    {
        if (isHolding && !pastHoldingThreshold)
            DecreaseHoldThreshold();
    }

    private void DecreaseHoldThreshold()
    {
        currentHoldThreshold -= Time.deltaTime;

        if (currentHoldThreshold <= 0)
        {
            OnHoldAction();
        } 
    }

    public void OnHoldAction()
    {
        pastHoldingThreshold = true;

        // Transition into throw.
        //print("START THROW!");
    }

    protected override void OnReleasedAction()
    {
        if (pastHoldingThreshold)
        {
            throwAction.AttemptThrow();
        }
        else
        {
            stabAction.AttemptStab();
        }
    }
}
