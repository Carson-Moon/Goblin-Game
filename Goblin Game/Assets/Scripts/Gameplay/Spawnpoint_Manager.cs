using Unity.Netcode;
using UnityEngine;

public class Spawnpoint_Manager : MonoBehaviour
{
    // Spawn point singleton
    public static Spawnpoint_Manager instance {get; private set;}

    [Header("Spawn Point Settings")]
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] private NetworkVariable<int> m_CurrentIndex = new NetworkVariable<int>(0);


    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Return the next spawn point.
    public Transform GetNextSpawnPosition()
    {
        Transform nextPosition = spawnPoints[m_CurrentIndex.Value];

        // Increase our current position index.
        IncreaseCurrentIndexRPC();

        return nextPosition;
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void IncreaseCurrentIndexRPC()
    {
        m_CurrentIndex.Value++;
    }
}
