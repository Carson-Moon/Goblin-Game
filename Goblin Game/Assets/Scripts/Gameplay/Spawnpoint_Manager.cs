using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Spawnpoint_Manager : NetworkBehaviour
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

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        m_CurrentIndex.OnValueChanged += OnValueChanged;
    }

    private void OnValueChanged(int wasInt, int newInt)
    {
        //Debug.Log(m_CurrentIndex.Value);
    }

    // Return the next spawn point.
    public Transform GetNextSpawnPosition()
    {
        Transform nextPosition = spawnPoints[m_CurrentIndex.Value];

        // Increase our current position index.
        IncreaseCurrentIndexRPC();

        return nextPosition;
    }

    [Rpc(SendTo.Server)]
    private void IncreaseCurrentIndexRPC()
    {
        m_CurrentIndex.Value += 1;
    }
}
