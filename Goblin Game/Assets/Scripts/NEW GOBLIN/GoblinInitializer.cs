using KinematicCharacterController;
using Unity.Netcode;
using UnityEngine;

// Used to initialize our goblin as the client or someone else.
// Turns on and off what we need for each scenario.

public class GoblinInitializer : NetworkBehaviour
{
    // Movement/Body
    [SerializeField] Goblin goblin;
    [SerializeField] GoblinCharacter goblinCharacter;
    [SerializeField] KinematicCharacterMotor kinematicCharacterMotor;
    [SerializeField] GameObject arms;

    // Camera
    [SerializeField] Camera goblinMainCamera;
    [SerializeField] Camera goblinArmCamera;
    [SerializeField] GoblinCamera goblinCamera;
    [SerializeField] AudioListener audioListener;

    // Input
    [SerializeField] GoblinInput goblinInput;
    [SerializeField] GoblinLeftClick goblinLeftClick;
    [SerializeField] GoblinRightClick goblinRightClick;
    [SerializeField] StabAction stabAction;
    [SerializeField] GrabAction grabAction;
    [SerializeField] VacuumAction vacuumAction;
    [SerializeField] ThrowAction throwAction;

    [SerializeField] bool overrideInitialization = false;

    void Awake()
    {
        if (overrideInitialization)
            return;

        goblin.enabled = false;
        goblinCharacter.enabled = false;
        kinematicCharacterMotor.enabled = false;
        arms.SetActive(false);

        goblinMainCamera.enabled = false;
        goblinArmCamera.enabled = false;
        goblinCamera.enabled = false;

        goblinInput.enabled = false;
        goblinLeftClick.enabled = false;
        goblinRightClick.enabled = false;
        stabAction.enabled = false;
        grabAction.enabled = false;
        vacuumAction.enabled = false;
        throwAction.enabled = false;
    }

    void Start()
    {
        // Move to a spawn point here probably eventually.
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            goblin.enabled = true;
            goblinCharacter.enabled = true;
            kinematicCharacterMotor.enabled = true;
            arms.SetActive(true);

            goblinMainCamera.enabled = true;
            goblinArmCamera.enabled = true;
            goblinCamera.enabled = true;

            goblinInput.enabled = true;
            goblinLeftClick.enabled = true;
            goblinRightClick.enabled = true;
            stabAction.enabled = true;
            grabAction.enabled = true;
            vacuumAction.enabled = true;
            throwAction.enabled = true;
        }
    }
}
