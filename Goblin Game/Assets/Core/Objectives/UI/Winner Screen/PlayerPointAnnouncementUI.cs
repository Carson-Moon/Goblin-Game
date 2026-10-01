using TMPro;
using UnityEngine;

public class PlayerPointAnnouncementUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI pointText;


    public void Initialize(string name, int points)
    {
        nameText.text = name;
        pointText.text = points.ToString();
    }
}
