using Unity.Netcode;
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

    [Header("Pickup")]
    [SerializeField] UnityEvent onPickup;
    [SerializeField] UnityEvent offPickup;

    [Header("Throw")]
    [SerializeField] UnityEvent onThrow;

    [Header("Vacuum")]
    [SerializeField] UnityEvent onVacuum;
    [SerializeField] UnityEvent offVacuum;

    [Header("Debug")]
    [SerializeField] UnityEvent onDebug;



    void Awake()
    {
        pControls = new PlayerControls();

        // Assign input listeners.
        // pControls.Goblin.Move.performed += ctx => rawMoveDirection = ctx.ReadValue<Vector2>();
        // pControls.Goblin.Look.performed += ctx => rawMouseDelta = ctx.ReadValue<Vector2>();

        // pControls.Goblin.Jump.started += ctx => onJump.Invoke();
        // pControls.Goblin.Jump.canceled += ctx => offJump.Invoke();

        // pControls.Goblin.Sprint.started += ctx => onSprint.Invoke();
        // pControls.Goblin.Sprint.canceled += ctx => offSprint.Invoke();

        // pControls.Goblin.Crouch.started += ctx => onCrouch.Invoke();
        // pControls.Goblin.Crouch.canceled += ctx => offCrouch.Invoke();

        // pControls.Goblin.Stab.started += ctx => onStab.Invoke();

        // pControls.Goblin.Pickup.started += ctx => onPickup.Invoke();
        // pControls.Goblin.Pickup.canceled += ctx => offPickup.Invoke();

        // pControls.Goblin.Throw.started += ctx => onThrow.Invoke();

        // pControls.Goblin.Vacuum.started += ctx => onVacuum.Invoke();
        // pControls.Goblin.Vacuum.canceled += ctx => offVacuum.Invoke();

        // pControls.Goblin.Debug.started += ctx => onDebug.Invoke();
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
