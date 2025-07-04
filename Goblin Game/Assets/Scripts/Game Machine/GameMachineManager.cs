using System;
using UnityEngine;

public class GameMachineManager : MonoBehaviour
{
    // Singleton
    public static GameMachineManager instance { get; private set; }

    [Header("Runtime")]
    [SerializeField] GameState startingState;
    [SerializeField] GameState currentState;
    [SerializeField] GameState nextState;
    [SerializeField] Action StartNextState;
    [SerializeField] Action EndCurrentState;


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

    void Start()
    {
        // Start our starting state.
        SetCurrentState(startingState);
        startingState.StartThisState();

        FadeUI.StartFade(0, 1);
    }

    // Set our current state.
    public void SetCurrentState(GameState current)
    {
        // Set our current state.
        currentState = current;

        // Clear and set our end current state.
        EndCurrentState = null;
        EndCurrentState += current.EndThisState;
    }

    // Set our next state.
    public void SetNextState(GameState next)
    {
        // Set our next state.
        nextState = next;

        // Clear and set our start next state action.
        StartNextState = null;
        StartNextState += next.StartThisState;
    }
}
