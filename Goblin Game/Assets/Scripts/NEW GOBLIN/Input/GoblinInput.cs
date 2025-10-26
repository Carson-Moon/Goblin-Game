using UnityEngine;
using UnityEngine.Events;

public class GoblinInput : MonoBehaviour
{
    PlayerControls pControls;

    [Header("Input Values")]
    [SerializeField] Vector2 rawMove;
    public Vector2 RawMove => rawMove;
    [SerializeField] Vector2 rawMouse;
    public Vector2 RawMouse => rawMouse;

    [Header("Input Events")]
    [SerializeField] UnityEvent onJump;
    [SerializeField] UnityEvent offJump;
    [SerializeField] UnityEvent onLeftClick;
    [SerializeField] UnityEvent offLeftClick;
    [SerializeField] UnityEvent onRightClick;
    [SerializeField] UnityEvent offRightClick;


    void Awake()
    {
        pControls = new PlayerControls();

        pControls.GoblinControls.Move.performed += ctx => rawMove = ctx.ReadValue<Vector2>();
        pControls.GoblinControls.Jump.started += ctx => onJump.Invoke();
        pControls.GoblinControls.Jump.canceled += ctx => offJump.Invoke();
        pControls.GoblinControls.LeftClick.started += ctx => onLeftClick.Invoke();
        pControls.GoblinControls.LeftClick.canceled += ctx => offLeftClick.Invoke();
        pControls.GoblinControls.RightClick.started += ctx => onRightClick.Invoke();
        pControls.GoblinControls.RightClick.canceled += ctx => offRightClick.Invoke();
    }


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
