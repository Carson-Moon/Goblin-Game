using UnityEngine;
using UnityEngine.Audio;

public class VoiceChatCanvas : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] AudioMixer voiceChatMixer;


    void Start()
    {
        OnSliderUpdated(0.5f);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
            ToggleCanvas();
    }

#region Voice Chat Settings UI
    public void OnSliderUpdated(float value)
    {
        voiceChatMixer.SetFloat("Volume", Mathf.Log10(value) * 20);
    }
#endregion
    
#region Show/Hide
    private bool visible = false;

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
#endregion
}
