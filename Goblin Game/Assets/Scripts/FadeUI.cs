using DG.Tweening;
using UnityEngine;

// Call from anywhere to fade in/out a black overlay.

public class FadeUI : MonoBehaviour
{
    [SerializeField] static CanvasGroup fadingCanvas;


    void Awake()
    {
        fadingCanvas = GetComponent<CanvasGroup>();
    }

    public static void StartFade(float alpha, float fadeTime)
    {
        fadingCanvas.DOFade(alpha, fadeTime);
    }
}
