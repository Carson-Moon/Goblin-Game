using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour
{
    [Header("Character Controllers")]
    [SerializeField] private GoblinCharacter goblinCharacter;
    [SerializeField] private GoblinCamera goblinCamera;
    [Header("Camera Effects")]
    [SerializeField] private CameraEffectStack cameraEffects;
    [SerializeField] private LandingDipEffect landingDipEffect;
    [SerializeField] private HandSway handSway;

    [SerializeField] private List<string> _movementLocks = new();
    [SerializeField] private List<string> _lookLocks = new();
    private bool CanMove => _movementLocks.Count == 0;
    private bool CanLook => _lookLocks.Count == 0;

    private PlayerControls pControls;

    void Start()
    {
        pControls = new PlayerControls();
        pControls.Enable();

        goblinCharacter.Initialize();
        goblinCamera.Initialize(goblinCharacter.CameraTarget);
        landingDipEffect.Initialize(goblinCharacter);
        cameraEffects.Initialize();
        handSway.Initialize();
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

        cameraEffects.Tick(deltaTime);

        var cameraInput = new CameraInput
        {
            Look = CanLook ? input.Look.ReadValue<Vector2>() : Vector2.zero
        };
        goblinCamera.UpdateRotation(cameraInput, cameraEffects.EulerOffset);

        var characterInput = new CharacterInput
        {
            Rotation = goblinCamera.transform.rotation,
            Move = CanMove ? input.Movement.ReadValue<Vector2>() : Vector2.zero,
            Jump = CanMove ? input.Jump.WasPressedThisFrame() : false,
            JumpSustain = CanMove ? input.Jump.IsPressed() : false,
            Crouch = input.Crouch.IsPressed()
        };
        
        goblinCharacter.UpdateInput(characterInput);
        goblinCharacter.UpdateBody(deltaTime);
    }

    void LateUpdate()
    {
        goblinCamera.UpdatePosition(goblinCharacter.CameraTarget, cameraEffects.PositionOffset);
        handSway.Tick(Time.deltaTime);
        // Debug.Log(cameraEffects.PositionOffset.y);
    }

    public void Teleport(Vector3 position)
    {
        goblinCharacter.SetPosition(position);
    }

    public void AddMovementLock(string lockID)
    {
        _movementLocks.Add(lockID);
    }

    public void RemoveMovementLock(string lockID)
    {
        _movementLocks.Remove(lockID);
    }

    public void AddLookLock(string lockID)
    {
        _lookLocks.Add(lockID);
    }

    public void RemoveLookLock(string lockID)
    {
        _lookLocks.Remove(lockID);
    }

    public void RemoveAllLocks()
    {
        _movementLocks.Clear();
        _lookLocks.Clear();
    }
}
