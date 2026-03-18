using TMPro;
using UnityEngine;

public class AwardsCeremonyUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI statAnnouncementText;
    [SerializeField] TextMeshProUGUI playerAnnouncementText;


    public void SetStatTitle(string title)
    {
        statAnnouncementText.text = title;
    }

    public void SetPlayerName(string name)
    {
        playerAnnouncementText.text = name;
    }

    public void ResetScreen()
    {
        statAnnouncementText.text = string.Empty;
        playerAnnouncementText.text = string.Empty;
    }
}
