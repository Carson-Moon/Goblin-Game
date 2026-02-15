using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] AudioListener audioListener;

    [Header("Menu Sections")]
    [SerializeField] MainMenuLobbySection lobbySection;
    [SerializeField] MainMenuUsernameSection usernameSection;
    


    void Start()
    {
        lobbySection.Initialize(Hide);
        usernameSection.Initialize();
    }

#region Show/Hide
    public void Show()
    {
        canvasGroup.TurnOn(1f);
        audioListener.enabled = true;
    }

    public void Hide()
    {
        canvasGroup.TurnOff(1f);
        audioListener.enabled = false;
    }

#endregion
}
