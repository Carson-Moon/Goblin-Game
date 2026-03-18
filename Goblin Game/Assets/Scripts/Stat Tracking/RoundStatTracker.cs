using Unity.Netcode;
using UnityEngine;

// Responsible for keeping track of our stats for this round!

[System.Serializable]
public struct RoundStats : INetworkSerializable
{
    // All of our VERY important stats!
    public int CoinsCollected;
    public int TotalCoinsAtEnd;
    public int TimesStabbedOtherPlayers;
    public int TimesStabbedByOtherPlayer;
    public float TimeKnockedOut;
    public int TimesKnockedOutOtherPlayer;
    public int TimesKnockedOutByOtherPlayer;
    public float TimeCrouching;
    public float TimeSprinting;
    public float TimeDoingNothing;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref CoinsCollected);
        serializer.SerializeValue(ref TotalCoinsAtEnd);
        serializer.SerializeValue(ref TimesStabbedOtherPlayers);
        serializer.SerializeValue(ref TimesStabbedByOtherPlayer);
        serializer.SerializeValue(ref TimeKnockedOut);
        serializer.SerializeValue(ref TimesKnockedOutOtherPlayer);
        serializer.SerializeValue(ref TimesKnockedOutByOtherPlayer);
        serializer.SerializeValue(ref TimeCrouching);
        serializer.SerializeValue(ref TimeSprinting);
        serializer.SerializeValue(ref TimeDoingNothing);
    }

    // Getters
    public int GetIntStat(IntStat stat)
    {
        switch (stat)
        {
            case IntStat.Coin:
                return CoinsCollected;

            case IntStat.Stabbed_Someone:
                return TimesStabbedOtherPlayers;

            case IntStat.Got_Stabbed:
                return TimesStabbedByOtherPlayer;

            case IntStat.Got_Knocked_Out:
                return TimesKnockedOutByOtherPlayer;

            case IntStat.Knocked_Someone_Out:
                return TimesKnockedOutOtherPlayer;

            default:
                Debug.LogWarning($"Could not find int stat: {stat}.");
                return 0;
        }
    }

    public float GetFloatStat(FloatStat stat)
    {
        switch (stat)
        {
            case FloatStat.Is_Knocked_Out:
                return TimeKnockedOut;

            case FloatStat.Is_Crouching:
                return TimeCrouching;

            case FloatStat.Is_Sprinting:
                return TimeSprinting;

            case FloatStat.Is_Doing_Nothing:
                return TimeDoingNothing;

            default:
                Debug.LogWarning($"Could not find float stat: {stat}.");
                return 0;
        }
    }
}

public enum IntStat
{
    Coin,
    Stabbed_Someone,
    Got_Stabbed,
    Knocked_Someone_Out,
    Got_Knocked_Out,

}

public enum FloatStat
{
    Is_Knocked_Out,
    Is_Crouching,
    Is_Sprinting,
    Is_Doing_Nothing
}

public class RoundStatTracker : MonoBehaviour
{
    // Singleton
    public static RoundStatTracker Instance { get; private set; }

    [Header("Current Stats")]
    public int coinsCollected;
    public int totalCoinsAtEnd;
    public int timesStabbedOtherPlayers;
    public int timesStabbedByOtherPlayer;
    public float timeKnockedOut;
    public int timesKnockedOutOtherPlayer;
    public int timesKnockedOutByOtherPlayer;
    public float timeCrouching;
    public float timeSprinting;
    public float timeDoingNothing;


    void Awake()
    {
        // Singleton Setup
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Create a round stats struct for this round.
    public RoundStats GrabRoundStats()
    {
        return new RoundStats()
        {
            CoinsCollected = coinsCollected,
            TotalCoinsAtEnd = totalCoinsAtEnd,
            TimesStabbedOtherPlayers = timesStabbedOtherPlayers,
            TimesStabbedByOtherPlayer = timesStabbedByOtherPlayer,
            TimeKnockedOut = timeKnockedOut,
            TimesKnockedOutOtherPlayer = timesKnockedOutOtherPlayer,
            TimesKnockedOutByOtherPlayer = timesKnockedOutByOtherPlayer,
            TimeCrouching = timeCrouching,
            TimeSprinting = timeSprinting,
            TimeDoingNothing = timeDoingNothing
        };
    }

    // Trackers
    public void TrackIntStat(IntStat stat)
    {
        switch (stat)
        {
            case IntStat.Coin:
                coinsCollected++;
                break;

            case IntStat.Stabbed_Someone:
                timesStabbedOtherPlayers++;
                break;

            case IntStat.Got_Stabbed:
                timesStabbedByOtherPlayer++;
                break;

            case IntStat.Got_Knocked_Out:
                timesKnockedOutByOtherPlayer++;
                break;

            case IntStat.Knocked_Someone_Out:
                timesKnockedOutOtherPlayer++;
                break;
        }
    }

    public void TrackFloatStat(FloatStat stat)
    {
        switch (stat)
        {
            case FloatStat.Is_Knocked_Out:
                timeKnockedOut += Time.deltaTime;
                break;

            case FloatStat.Is_Crouching:
                timeCrouching += Time.deltaTime;
                break;

            case FloatStat.Is_Sprinting:
                timeSprinting += Time.deltaTime;
                break;

            case FloatStat.Is_Doing_Nothing:
                timeDoingNothing += Time.deltaTime;
                break;
        }
    }
}
