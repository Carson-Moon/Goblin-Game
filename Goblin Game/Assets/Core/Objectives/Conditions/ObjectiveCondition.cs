using System;
using Unity.Netcode;
using UnityEngine;

public abstract class ObjectiveCondition : NetworkBehaviour
{
    public void UpdateConditionUI()
    {
        ObjectiveCanvas.Instance.UpdateConditionPanel(this);
    }

    public void SetupConditionServer()
    {
        OnSetupConditionServer();
        OnSetupConditionClientRpc();
    }

    protected abstract void OnSetupConditionServer();

    [ClientRpc]
    protected virtual void OnSetupConditionClientRpc()
    {
        
    }


    public abstract string GetPanelDisplay();
}
