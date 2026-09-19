using System.Collections;
using UnityEngine;

public class NetworkAutostarter : MonoBehaviour
{
    [SerializeField] RelayConnection relayConnection;
    [SerializeField] string joinCode;



    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            relayConnection.StartHosting(null);
        }

        if(Input.GetKeyDown(KeyCode.J))
        {
            relayConnection.JoinRelayAsClient(joinCode, null);
        }
    }
}
