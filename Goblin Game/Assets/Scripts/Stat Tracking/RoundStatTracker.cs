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
}

public enum IntStat
{
    Coin,
    StabbedSomeone,
    GotStabbed,
    KnockedSomeoneOut,
    GotKnockedOut,

}

public enum FloatStat
{
    IsKnockedOut,
    IsCrouching,
    IsSprinting,
    IsDoingNothing
}

public class RoundStatTracker : MonoBehaviour
{
    // Singleton
    public static RoundStatTracker instance { get; private set; }

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
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
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

            case IntStat.StabbedSomeone:
                timesStabbedOtherPlayers++;
                break;

            case IntStat.GotStabbed:
                timesStabbedByOtherPlayer++;
                break;

            case IntStat.GotKnockedOut:
                timesKnockedOutByOtherPlayer++;
                break;

            case IntStat.KnockedSomeoneOut:
                timesKnockedOutOtherPlayer++;
                break;
        }
    }

    public void TrackFloatStat(FloatStat stat)
    {
        switch (stat)
        {
            case FloatStat.IsKnockedOut:
                timeKnockedOut += Time.deltaTime;
                break;

            case FloatStat.IsCrouching:
                timeCrouching += Time.deltaTime;
                break;

            case FloatStat.IsSprinting:
                timeSprinting += Time.deltaTime;
                break;

            case FloatStat.IsDoingNothing:
                timeDoingNothing += Time.deltaTime;
                break;
        }
    }
}
