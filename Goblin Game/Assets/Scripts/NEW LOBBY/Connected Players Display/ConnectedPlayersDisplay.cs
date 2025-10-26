using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class ConnectedPlayersDisplay : NetworkBehaviour
{
    [SerializeField] ConnectedPlayerUI nameUI;
    [SerializeField] RectTransform nameHolder;
    private Dictionary<ulong, ConnectedPlayerUI> connectedPlayerDisplays = new();


    [ClientRpc]
    public void AddConnectedPlayerDisplayClientRpc(ulong clientID, FixedString32Bytes username, ClientRpcParams clientRpcParams)
    {
        ConnectedPlayerUI newPlayerUI = Instantiate(nameUI, nameHolder);
        newPlayerUI.Setup(username, clientID);

        connectedPlayerDisplays.Add(clientID, newPlayerUI);
    }

    [ClientRpc]
    public void RemoveConnectedPlayerDisplayClientRpc(ulong clientID, ClientRpcParams clientRpcParams)
    {
        ConnectedPlayerUI playerToRemove = connectedPlayerDisplays[clientID];
        connectedPlayerDisplays.Remove(clientID);
        Destroy(playerToRemove);
    }
}
