using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ServerGameMachineManager : NetworkBehaviour
{
    // Singleton
    public static ServerGameMachineManager instance { get; private set; }

    public ClientGameMachine clientMachine;
    public ConnectedPlayerManager connectedPlayers;

    [Header("Runtime")]
    [SerializeField] GameState startingState;
    [SerializeField] GameState currentState;
    [SerializeField] GameState nextState;
    [SerializeField] Action StartNextState;
    [SerializeField] Action EndCurrentState;

    [Header("State Switch Cooldown")]
    [SerializeField] bool justSwitchedStates = false;
    [SerializeField] float stateSwitchCooldownLength;
    private float stateSwitchCooldown;


    void Awake()
    {
        #region Singleton Setup
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        #endregion

    }

    void Update()
    {
        if (justSwitchedStates)
        {
            stateSwitchCooldown -= Time.deltaTime;
            if (stateSwitchCooldown <= 0)
            {
                justSwitchedStates = false;
            }
        }
    }

    // Attempt a state switch for all of the clients.
    public void AttemptStateSwitch()
    {
        print("ATTEMPTING");

        if (!IsServer || justSwitchedStates)
        {
            return;
        }

        // Check to ensure all of our clients are ready to switch!
        if (!CheckClientsReadiness())
            return;

        // Apply our state switch cooldown.
        stateSwitchCooldown = stateSwitchCooldownLength;
        justSwitchedStates = true;

        StartClientStateRPC();
    }

    // Return if all of our clients are ready to switch states or not!
    private bool CheckClientsReadiness()
    {
        Dictionary<ulong, ClientMachineInteractor> machineInteractors = connectedPlayers.GetClientInteractors();
        List<ulong> clientIDs = connectedPlayers.GetClientIDs();

        foreach (ulong id in clientIDs)
        {
            if (machineInteractors.TryGetValue(id, out ClientMachineInteractor clientInteractor))
            {
                if (clientInteractor.CheckStateSwitchReadiness())
                {
                    continue;
                }
                else
                {
                    Debug.LogWarning($"Client {id} is not ready to switch states!");
                    return false;
                }
            }
        }

        return true;
    }

    // Called when a client just finished a state.
    [Rpc(SendTo.Server)]
    public void OnClientReadyToSwitchRPC()
    {
        AttemptStateSwitch();
    }

    // Start our next state on the client side.
    [Rpc(SendTo.ClientsAndHost)]
    private void StartClientStateRPC()
    {
        clientMachine.StartNextState();
    }
}
