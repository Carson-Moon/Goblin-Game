using TMPro;
using UnityEngine;

public class LobbyUIComponent : UIComponent
{
    [SerializeField] CanvasGroup lobbyCG;

    [Header("UI Input")]
    [SerializeField] Animated_Button hostButton;
    [SerializeField] Animated_Button joinButton;
    [SerializeField] TMP_InputField joinCodeField;


    public override void Initialize()
    {
        hostButton.onButtonPressedAction += Host;
        joinButton.onButtonPressedAction += Join;

        Show();
    }

    public override void Show()
    {
        lobbyCG.TurnOn();
    }

    public override void Hide()
    {
        lobbyCG.TurnOff();
    }

    private void Host()
    {
        RelayConnection.Instance.StartHosting(Hide);
    }

    private void Join()
    {
        if(joinCodeField.text.Length < 6)
            return;

        RelayConnection.Instance.StartJoining(joinCodeField.text.Trim().ToUpper(), Hide);
    }
}
