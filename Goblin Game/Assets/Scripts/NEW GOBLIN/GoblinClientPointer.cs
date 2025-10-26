using Unity.Netcode;
using UnityEngine;

public static class GoblinClientPointer
{
    private static GoblinClient localGoblinClient = null;

    public static GoblinClient LocalGoblinClient()
    {
        if (localGoblinClient == null)
        {
            localGoblinClient = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<GoblinClient>();
        }


        return localGoblinClient;
    }
}
