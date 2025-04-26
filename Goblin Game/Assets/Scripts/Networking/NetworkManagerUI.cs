using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] Button serverButton;
    [SerializeField] Button clientButton;


    void Awake()
    {
        serverButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });

        clientButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });
    }
}
