using TMPro;
using UnityEngine;

public class ObjectivePanelUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;


    public void Initialize(string display)
    {
        text.text = display;
    }
}
