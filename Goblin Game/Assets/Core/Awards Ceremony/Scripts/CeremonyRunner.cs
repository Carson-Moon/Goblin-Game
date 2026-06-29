using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class CeremonyRunner : NetworkBehaviour
{
    [SerializeField] CeremonyState startingState;

    private CeremonyState runningState = null;


    void Start()
    {
        RunState(startingState);
    }

    private void RunState(CeremonyState stateToRun)
    {
        runningState = stateToRun;
        runningState.StartState();
        StartCoroutine(UpdateState());
    }

    IEnumerator UpdateState()
    {
        while(runningState.Running)
        {
            runningState.UpdateState();
            yield return null;
        }

        if(runningState.nextState != null)
        {
            Debug.Log("Go to next state!");
            RunState(runningState.nextState);
        }
        else
        {
            Debug.Log("End of ceremony!");
        }
    }
}
