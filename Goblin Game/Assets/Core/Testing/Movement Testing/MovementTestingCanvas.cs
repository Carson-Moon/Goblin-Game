using UnityEngine;

public class MovementTestingCanvas : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] string url;
    private bool showing = false;


    void Awake()
    {
        showing = !(canvasGroup.alpha > 0);
        ToggleMenu();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
            ToggleMenu();
    }

    private void ToggleMenu()
    {
        showing = !showing;
        canvasGroup.alpha = showing ? 1 : 0;
        canvasGroup.blocksRaycasts = showing;
        Cursor.lockState = showing ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void OpenGoogleForm()
    {
        Application.OpenURL(url);
    }
}
