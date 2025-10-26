using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] CanvasGroup mainCanvasGroup;
    [SerializeField] CanvasGroup usernameCanvasGroup;

    [SerializeField] UnityEvent OnAbleToHost;
    [SerializeField] UnityEvent OnAbleToJoin;


    public void DisableMainMenu()
    {
        mainCanvasGroup.interactable = false;
        mainCanvasGroup.blocksRaycasts = false;

        mainCanvasGroup.DOFade(0, 1f);
    }

    public void EnableMainMenu()
    {
        mainCanvasGroup.interactable = true;
        mainCanvasGroup.blocksRaycasts = true;

        mainCanvasGroup.DOFade(1, 1f);
    }

    public void DisableUsernameMenu()
    {
        usernameCanvasGroup.interactable = false;
        usernameCanvasGroup.blocksRaycasts = false;

        usernameCanvasGroup.DOFade(0, 1f);
    }

    public void EnableUsernameMenu()
    {
        usernameCanvasGroup.interactable = true;
        usernameCanvasGroup.blocksRaycasts = true;

        usernameCanvasGroup.DOFade(1, 1f);
    }

    public void AttemptToHostLobby()
    {
        if (!HasValidUsername())
        {
            // Enable our username select popup.
            EnableUsernameMenu();
            return;
        }

        OnAbleToHost.Invoke();
    }

    public void AttemptToJoinLobby()
    {
        if (!HasValidUsername())
        {
            // Enable our username select popup.
            EnableUsernameMenu();
            return;
        }

        OnAbleToJoin.Invoke();
    }

    private bool HasValidUsername()
    {
        if (PlayerPrefs.GetString(StaticPlayerPrefsHelper.UsernamePref, StaticPlayerPrefsHelper.DefaultUsername) == StaticPlayerPrefsHelper.DefaultUsername)
        {
            // This means we have not chosen a username yet.
            return false;
        }

        return true;
    }
}
