using UnityEngine;
using Unity.Netcode;

// Responsible for grabbing, holding, and returning our client goblin.

public class ClientGoblinHelper : MonoBehaviour
{
    private static Client_Goblin clientGoblin;


    // Either fetches our client goblin and returns, or returns the stored client goblin.
    public static Client_Goblin GetMyClientGoblin()
    {
        if (clientGoblin != null)
        {
            return clientGoblin;
        }

        // Grab our client goblin since we don't have a reference to it already.
        clientGoblin = ConnectedPlayerManager.instance.GetClientGoblin(
            NetworkManager.Singleton.LocalClientId
        );
        return clientGoblin;
    }

    // Sets our goblin client to the one passed in.
    public static void SetMyClientGoblin(Client_Goblin _clientGoblin)
    {
        clientGoblin = _clientGoblin;
    }
}
