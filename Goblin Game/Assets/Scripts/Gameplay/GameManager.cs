using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [Header("Game Timer")]
    [SerializeField] private bool m_RunTimer = false;
    [SerializeField] private float timerLength;
    [SerializeField] private NetworkVariable<float> m_Timer = new NetworkVariable<float>();


    void Start()
    {
        InitializeTimer();
    }

    void Update()
    {
        if(IsServer && m_RunTimer)
        {
            m_Timer.Value -= Time.deltaTime;

            if(m_Timer.Value <= 0)
            {
                m_Timer.Value = 0;
                m_RunTimer = false;
                OnTimerEnd();
            }
        }
    }

    // Initialize our timer.
    public void InitializeTimer()
    {
        if(IsServer)
        {
            m_Timer.Value = timerLength;
            m_RunTimer = true;
        }
    }

    // Call this to pause the timer.
    public void PauseTimer()
    {

    }

    // Call this when the timer ends.
    private void OnTimerEnd()
    {

    }

    // Call this to get the current value of the timer.
    public float GetTimerValue()
    {
        return m_Timer.Value;
    }
}
