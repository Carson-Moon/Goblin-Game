using System;
using UnityEngine;

public class AltarInteractable : Interactable
{
    public Animator animator;

    public event Action onInteracted;
    private AltarController altarController;


    public void Initialize(AltarController controller, Action onInteracted)
    {
        this.onInteracted = onInteracted;
        altarController = controller;
    }

    void OnDestroy()
    {
        onInteracted = null;
    }

    public override void Interact()
    {
        if(!altarController.AltarsAvailable)
            return;

        base.Interact();

        onInteracted?.Invoke();

        AltarUI.Instance.Show();
    }
}
