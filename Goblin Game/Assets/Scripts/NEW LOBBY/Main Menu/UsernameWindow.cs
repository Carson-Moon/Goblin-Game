using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UsernameWindow : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;

    [SerializeField] UnityEvent onSetNewUsername;


    public void SetNewUsername()
    {
        if (inputField.text.Length <= 0)
            return;

        PlayerPrefs.SetString(StaticPlayerPrefsHelper.UsernamePref, inputField.text);
        onSetNewUsername.Invoke();
    }
}
