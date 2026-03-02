using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PickupVisuals : NetworkBehaviour
{
    [SerializeField] List<PickupVisual> firstPersonVisuals = new();
    [SerializeField] List<PickupVisual> thirdPersonVisuals = new();


    [Rpc(SendTo.ClientsAndHost)]
    public void TogglePickupVisualClientRpc(PickupID id)
    {
        TogglePickupVisual(id);
    }

    private void TogglePickupVisual(PickupID id)
    {
        if(id is PickupID.None)
        {
            foreach(var fpVisual in firstPersonVisuals)
                fpVisual.ToggleVisual(false);
            
            foreach(var tpVisual in thirdPersonVisuals)
                tpVisual.ToggleVisual(false);
        }
        else
        {
            if(IsOwner)
            {
                foreach(var fpVisual in firstPersonVisuals)
                {
                    if(fpVisual.ID == id)
                        fpVisual.ToggleVisual(true);
                    else
                        fpVisual.ToggleVisual(false);
                }
            }
            else
            {
                foreach(var tpVisual in thirdPersonVisuals)
                {
                    if(tpVisual.ID == id)
                        tpVisual.ToggleVisual(true);
                    else
                        tpVisual.ToggleVisual(false);
                }
            }
        }
    }
}
