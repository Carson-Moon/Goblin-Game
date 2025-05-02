using UnityEngine;

public class PlayerInformation_Manager : MonoBehaviour
{
    // Singleton
    public static PlayerInformation_Manager instance {get; private set;}

    [Header("Player Information Settings")]
    [SerializeField] private string m_PlayerName;


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

    // Called by the input field to set our player name.
    public void SetPlayerName(string value)
    {
        m_PlayerName = value;
    }

    public string GetPlayerName()
    {
        return m_PlayerName;
    }
}
