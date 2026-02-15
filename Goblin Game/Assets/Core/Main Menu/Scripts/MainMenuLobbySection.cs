using System;
using TMPro;
using UnityEngine;

public class MainMenuLobbySection : MonoBehaviour
{
    [Header("Lobby UI")]
    [SerializeField] Animated_Button createButton;
    [SerializeField] Animated_Button joinButton;
    [SerializeField] TMP_InputField joinCodeInputField;

    private Action onSuccess;


    public void Initialize(Action _onSuccess)
    {
        onSuccess = _onSuccess;

        createButton.onButtonPressedAction += Create;
    }

#region Create Button
    private void Create()
    {
        RelayConnection.Instance.StartHosting(onSuccess);
    }
#endregion

#region Join Button
    private void Join()
    {
        RelayConnection.Instance.StartJoining(joinCodeInputField.text.Trim().ToUpper(), onSuccess);
    }

#endregion
}
