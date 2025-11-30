using UnityEngine;
using UnityEngine.PlayerLoop;

public class Goblin : MonoBehaviour
{
    [Header("Character Controllers")]
    [SerializeField] private GoblinCharacter goblinCharacter;
    [SerializeField] private GoblinCamera goblinCamera;


    private PlayerControls pControls;

    void Start()
    {
        pControls = new PlayerControls();
        pControls.Enable();

        goblinCharacter.Initialize();
        goblinCamera.Initialize(goblinCharacter.CameraTarget);
    }

    void OnDestroy()
    {
        if(pControls != null)
            pControls.Dispose();
    }

    void Update()
    {
        var input = pControls.GoblinMovement;
        var deltaTime = Time.deltaTime;

        var cameraInput = new CameraInput
        {
            Look = input.Look.ReadValue<Vector2>()
        };
        goblinCamera.UpdateRotation(cameraInput);

        var characterInput = new CharacterInput
        {
            Rotation = goblinCamera.transform.rotation,
            Move = input.Movement.ReadValue<Vector2>(),
            Jump = input.Jump.WasPressedThisFrame(),
            JumpSustain = input.Jump.IsPressed(),
            Crouch = input.Crouch.WasPressedThisFrame()
                ? CrouchInput.Toggle
                : CrouchInput.None
        };
        goblinCharacter.UpdateInput(characterInput);
        goblinCharacter.UpdateBody(deltaTime);
    }

    void LateUpdate()
    {
        goblinCamera.UpdatePosition(goblinCharacter.CameraTarget);
    }

    public void Teleport(Vector3 position)
    {
        goblinCharacter.SetPosition(position);
    }
}
