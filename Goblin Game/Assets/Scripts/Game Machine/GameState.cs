using System;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public bool inState = false;

    public GameState nextState;
    public Action onStateEnding;

    public virtual void StartThisState()
    {
        print("Starting this state...");

        inState = true;
    }

    public virtual void EndThisState()
    {
        print("Ending this state...");

        inState = false;

        onStateEnding.Invoke();
    }
}
