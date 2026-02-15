using UnityEngine;

public class MainMenuSection : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;


    public void Show()
    {
        canvasGroup.TurnOn(1f);
    }

    public void Hide()
    {
        canvasGroup.TurnOff(1f);
    }
}
