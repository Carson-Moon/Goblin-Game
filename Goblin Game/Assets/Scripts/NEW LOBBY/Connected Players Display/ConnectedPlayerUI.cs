using TMPro;
using Unity.Collections;
using UnityEngine;

public class ConnectedPlayerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    private ulong clientID;
    public ulong ClientID => clientID;

    public void Setup(FixedString32Bytes name, ulong clientID)
    {
        nameText.text = name.ToString();
    }
}
