using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Menu Sections")]
    [SerializeField] MainMenuLobbySection lobbySection;
    


    void Start()
    {
        lobbySection.Initialize(Hide);
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
