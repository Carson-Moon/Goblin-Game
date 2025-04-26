using UnityEngine;
using UnityEngine.Events;

public class Input_Goblin : MonoBehaviour
{
    // Runtime
    PlayerControls pControls;

    [Header("Input Containers")]
    [SerializeField] Vector2 rawMoveDirection;
    [SerializeField] Vector2 rawMouseDelta;

    [Header("Input Events")]
    [Header("Jump")]
    [SerializeField] UnityEvent onJump;
    [SerializeField] UnityEvent offJump;

    [Header("Sprint")]
    [SerializeField] UnityEvent onSprint;
    [SerializeField] UnityEvent offSprint;

    [Header("Crouch")]
    [SerializeField] UnityEvent onCrouch;
    [SerializeField] UnityEvent offCrouch;

    [Header("Stab")]
    [SerializeField] UnityEvent onStab;


    void Awake()
    {
        pControls = new PlayerControls();

        // Assign input listeners.
        pControls.Goblin.Move.performed += ctx => rawMoveDirection = ctx.ReadValue<Vector2>();
        pControls.Goblin.Look.performed += ctx => rawMouseDelta = ctx.ReadValue<Vector2>();

        pControls.Goblin.Jump.started += ctx => onJump.Invoke();
        pControls.Goblin.Jump.canceled += ctx => offJump.Invoke();

        pControls.Goblin.Sprint.started += ctx => onSprint.Invoke();
        pControls.Goblin.Sprint.canceled += ctx => offSprint.Invoke();

        pControls.Goblin.Crouch.started += ctx => onCrouch.Invoke();
        pControls.Goblin.Crouch.canceled += ctx => offCrouch.Invoke();

        pControls.Goblin.Stab.started += ctx => onStab.Invoke();
    }

#region Getters
    public Vector2 GetRawMoveDirection()
    {
        return rawMoveDirection;
    }

    public Vector2 GetRawMouseDelta()
    {
        return rawMouseDelta;
    }

#endregion

#region Enable/Disable
    void OnEnable()
    {
        pControls.Enable();
    }

    void OnDisable()
    {
        pControls.Disable();
    }
#endregion
}
