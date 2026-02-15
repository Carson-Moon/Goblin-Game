using TMPro;
using UnityEngine;

public class MainMenuUsernameSection : MainMenuSection
{
    [Header("Username UI")]
    [SerializeField] Animated_Button usernameButton;
    [SerializeField] TextMeshProUGUI usernameButtonText;
    [SerializeField] Animated_Button submitUsernameButton;
    [SerializeField] TMP_InputField usernameInputField;
    

    public void Initialize()
    {
        string username = UsernameHolder.Username;

        usernameButtonText.text = string.IsNullOrEmpty(username) ? "Set Username" : username;
        usernameButton.onButtonPressedAction += Show;

        submitUsernameButton.onButtonPressedAction += CheckUsernameSubmission;
    }

    private void CheckUsernameSubmission()
    {
        if(string.IsNullOrEmpty(usernameInputField.text))
        {
            Debug.LogWarning("Cannot submit an empty username!");
        }
        else
        {
            UsernameHolder.SetNewUsername(usernameInputField.text);
            usernameButtonText.text = usernameInputField.text;

            Hide();
        }
    }
}
