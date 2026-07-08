using TMPro;
using UnityEngine;

public class UsernameSetter : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;


    void Start()
    {
        SetExistingUsername();
        inputField.onValueChanged.AddListener(OnValueChanged);
    }

    private void SetExistingUsername()
    {
        string existingUsername = UsernameHolder.FetchExistingUsername();
        inputField.text = existingUsername;
    }

    public void OnValueChanged(string value)
    {
        UsernameHolder.SetNewUsername(value);
    }
}
