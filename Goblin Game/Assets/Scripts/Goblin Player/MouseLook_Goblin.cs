using UnityEngine;

public class MouseLook_Goblin : MonoBehaviour
{
    // Runtime
    Input_Goblin pInput;

    [Header("Mouse Look Toggle")]
    [SerializeField] bool canLook = true;

    [Header("Mouse Movement")]
    [SerializeField] float sensitivity;
    [SerializeField] Vector2 mouseSmoothing;
    public Transform orientation;
    Vector2 rawMouse;
    Vector2 smoothMouse;
    Vector2 actualMouse;

    
    private void Awake()
    {
        pInput = GetComponentInParent<Input_Goblin>();

        // Lock Cursor
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void Update()
    {
        if(canLook)
            MouseLook();
    }

#region Mouse Look
    // Update mouse look based on player input. Update player graphics to face with the camera.
    private void MouseLook()
    {
        // Get mouse input.
        rawMouse = pInput.GetRawMouseDelta() * Time.deltaTime;

        // Smooth mouse movement.
        smoothMouse.x = Mathf.Lerp(smoothMouse.x, rawMouse.x, 1f / mouseSmoothing.x);
        smoothMouse.y = Mathf.Lerp(smoothMouse.y, rawMouse.y, 1f / mouseSmoothing.y);

        // Add to actual mouse look position.
        actualMouse += smoothMouse * sensitivity;

        actualMouse.y = Mathf.Clamp(actualMouse.y, -90f, 90f);

        // Rotate camera and orientation.
        transform.rotation = Quaternion.Euler(-actualMouse.y, actualMouse.x, 0);
        orientation.rotation = Quaternion.Euler(0, actualMouse.x, 0);
    }
#endregion
}
