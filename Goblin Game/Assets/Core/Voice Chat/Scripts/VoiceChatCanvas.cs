using UnityEngine;

public class VoiceChatCanvas : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    private bool visible = false;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
            ToggleCanvas();
    }

    public void ToggleCanvas()
    {
        visible = !visible;

        if(visible)
            Show();
        else
            Hide();
    }

    public void Show()
    {
        canvasGroup.TurnOn();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Hide()
    {
        canvasGroup.TurnOff();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
