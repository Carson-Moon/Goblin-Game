using UnityEngine;

public class ServerRoundTracker : MonoBehaviour
{
    public static ServerRoundTracker Instance {get; private set;}

    [SerializeField] int totalRounds = 2;

    private int _roundsPlayed = 0;
    public int RoundsPlayed => _roundsPlayed;

    public bool PlayedAllRounds => _roundsPlayed == totalRounds;


    void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void IncreaseRoundsPlayed()
    {
        _roundsPlayed++;
    }
}
