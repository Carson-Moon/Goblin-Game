using KinematicCharacterController;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

// Used to initialize our goblin as the client or someone else.
// Turns on and off what we need for each scenario.

public class GoblinInitializer : NetworkBehaviour
{
    // Movement
    [SerializeField] GoblinController goblin;
    [SerializeField] GoblinCharacter goblinCharacter;
    [SerializeField] KinematicCharacterMotor kinematicCharacterMotor;

    // Camera
    [SerializeField] Camera goblinMainCamera;
    [SerializeField] Camera goblinArmCamera;
    [SerializeField] CinemachineCamera cineCamera;
    [SerializeField] CinemachineBrain cineBrain;
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

    // Graphics
    [SerializeField] GameObject[] thirdPersonBody;
    [SerializeField] GameObject arms;
    [SerializeField] GoblinAnimator goblinAnimator;

    // UI
    [SerializeField] GameObject coinUICanvas;

    [SerializeField] bool overrideInitialization = false;

    void Awake()
    {
        if (overrideInitialization)
            return;

        goblin.enabled = false;
        goblinCharacter.enabled = false;
        kinematicCharacterMotor.enabled = false;

        goblinMainCamera.enabled = false;
        goblinArmCamera.enabled = false;
        cineCamera.enabled = false;
        cineBrain.enabled = false;
        goblinCamera.enabled = false;

        goblinInput.enabled = false;
        goblinLeftClick.enabled = false;
        goblinRightClick.enabled = false;
        stabAction.enabled = false;
        grabAction.enabled = false;
        vacuumAction.enabled = false;
        throwAction.enabled = false;

        arms.SetActive(false);
        goblinAnimator.enabled = false;

        coinUICanvas.SetActive(false);
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

            goblinMainCamera.enabled = true;
            goblinArmCamera.enabled = true;
            cineCamera.enabled = true;
            cineBrain.enabled = true;
            goblinCamera.enabled = true;

            goblinInput.enabled = true;
            goblinLeftClick.enabled = true;
            goblinRightClick.enabled = true;
            stabAction.enabled = true;
            grabAction.enabled = true;
            vacuumAction.enabled = true;
            throwAction.enabled = true;

            arms.SetActive(true);
            goblinAnimator.enabled = true;

            coinUICanvas.SetActive(true);
        }
        else
        {
            Destroy(audioListener);

            kinematicCharacterMotor.gameObject.layer = 7; // Network Goblin Layer

            foreach (GameObject bodyComponent in thirdPersonBody)
            {
                bodyComponent.gameObject.layer = 0;
            }
        }
    }
}
