using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Objective : NetworkBehaviour
{
    [SerializeField] string objectiveName;
    public string ObjectiveName => objectiveName;

    [SerializeField] string objectiveDescription;
    public string ObjectiveDescription => objectiveDescription;

    [SerializeField] ObjectiveCondition[] conditions;
    public ObjectiveCondition[] Conditions => conditions;

    [Header("Timer")]
    [SerializeField] TextMeshProUGUI timerDisplay;
    [SerializeField] float timerLength;
    private float timer;

    public event Action<List<ulong>> NotifyServerObjectiveCompleted;


    public void StartObjectiveServer(Action<List<ulong>> onComplete)
    {
        NotifyServerObjectiveCompleted = null;
        NotifyServerObjectiveCompleted += onComplete;

        OnStartObjectiveServer();
        OnStartObjectiveClientRpc();
    }

    private void OnStartObjectiveServer()
    {
        foreach(var condition in conditions)
            condition.SetupConditionServer();
    }

    [ClientRpc]
    private void OnStartObjectiveClientRpc()
    {
        ObjectiveCanvas.Instance.Initialize(this);

        StartObjectiveTimer();
    }

    public void EndObjectiveServer()
    {
        
    }

    private void OnEndObjectiveServer()
    {
        
    }

    [ClientRpc]
    private void OnEndObjectiveClientRpc()
    {
        
    }

    public void CleanUpObjective()
    {
        foreach(var condition in conditions)
            condition.CleanUpCondition();
    }

#region Timer
    private void StartObjectiveTimer()
    {
        StartCoroutine(ObjectiveTimer());
    }

    IEnumerator ObjectiveTimer()
    {
        timer = timerLength;

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            timerDisplay.text = timer.ToString("F0");
            yield return null;
        }

        NotifyServerObjectiveCompleted?.Invoke(new List<ulong> {});
    }

#endregion
}
