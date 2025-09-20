using UnityEngine;

// Base class for windows that perform some sort of animation to open and close.

public class Abstract_Toggle_Window : MonoBehaviour
{
    [SerializeField] protected bool isHidden = true;
    public bool startHidden = true;

    public virtual void ToggleWindow()
    {

    }

    public virtual void OpenWindow()
    {

    }

    public virtual void CloseWindow()
    {

    }
}
