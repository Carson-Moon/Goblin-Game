using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

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
    }

    public void Hide()
    {
        canvasGroup.TurnOff(1f);
    }

#endregion
}
