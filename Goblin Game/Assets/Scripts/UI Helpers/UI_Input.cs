using UnityEngine;
using UnityEngine.InputSystem;

// Universal holder for our UI input handling.

public class UI_Input : MonoBehaviour
{
    // Runtime
    static PlayerControls pControls;

    private static Vector2 mousePosition;
    private static Vector2 mouseDelta;


    void Awake()
    {
        pControls = new PlayerControls();

        pControls.UI_Interaction.MousePosition.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
        pControls.UI_Interaction.MouseDelta.performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();
    }

    public static Vector2 GetMousePosition()
    {
        return mousePosition;
    }

    public static Vector2 GetMouseDelta()
    {
        return mouseDelta;
    }

    void OnEnable()
    {
        pControls.Enable();
    }

    void OnDisable()
    {
        pControls.Disable();
    }
}
