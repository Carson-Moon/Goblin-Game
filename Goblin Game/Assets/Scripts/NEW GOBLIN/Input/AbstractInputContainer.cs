using UnityEngine;

public abstract class AbstractButtonInputContainer : MonoBehaviour
{
    [SerializeField] bool isDisabled = false;
    public bool IsDisabled => isDisabled;

    [SerializeField] bool isPressed = false;
    public bool IsPressed => isPressed;

    public void OnPressed()
    {
        if (isDisabled)
            return;

        isPressed = true;

        OnPressedAction();
    }

    protected virtual void OnPressedAction()
    {
        Debug.LogWarning("Pressed action is not setup here.");
    }

    public void OnReleased()
    {
        if (isDisabled)
            return;

        isPressed = false;

        OnReleasedAction();
    }

    protected virtual void OnReleasedAction()
    {
        Debug.LogWarning("Released action is not setup here.");
    }

    public void DisableInput()
    {
        isDisabled = true;
    }

    public void EnableInput()
    {
        isDisabled = false;
    }
}
