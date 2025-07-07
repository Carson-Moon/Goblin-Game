using UnityEngine;
using System;

public class ClientGameMachine : MonoBehaviour
{
    // Singleton
    public static ClientGameMachine instance { get; private set; }

    [SerializeField] ServerGameMachineManager serverMachine;

    [Header("Runtime")]
    [SerializeField] GameState startingState;
    [SerializeField] GameState currentState;
    [SerializeField] GameState nextState;

    [SerializeField] Action onStartNextState;
    [SerializeField] Action onEndNextState;

    [SerializeField] public bool readyToSwitch;


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

    // Alert our server machine that we have completed our state.
    public void OnCurrentStateCompleted()
    {
        readyToSwitch = true;

        serverMachine.OnClientReadyToSwitchRPC();
    }

    public void StartNextState()
    {
        readyToSwitch = false;

        print("STARTING CLIENT STATE!");

        // Assign the next states ending action to our on current state completed.
        if(nextState) nextState.onStateEnding = null;
        if(currentState) currentState.onStateEnding = null;
        if(nextState) nextState.onStateEnding += OnCurrentStateCompleted;

        currentState = nextState;
        nextState = currentState.nextState;

        // Actually start the next state!
        currentState.StartThisState();
    }
}
