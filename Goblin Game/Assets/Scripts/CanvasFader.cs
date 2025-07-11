using DG.Tweening;
using UnityEngine;

// Call from anywhere to fade in/out a black overlay.

public enum FadeLevel
{
    FullyTransparent,
    FullyOpaque
}

public enum FadeSpeed
{
    Slow,
    Medium,
    Fast,
    SuperFast
}

public class CanvasFader : MonoBehaviour
{
    [SerializeField] static CanvasGroup fadingCanvas;


    void Awake()
    {
        fadingCanvas = GetComponent<CanvasGroup>();
    }

    // Fades the canvas this is attached to. Fade to black canvas.
    public static void FadeCanvas(FadeLevel fadeLevel, FadeSpeed fadeSpeed)
    {
        fadingCanvas.DOFade((int)fadeLevel, DetermineFadeTime(fadeSpeed));
    }

    // Fades whatever canvas is passed in.
    public static void FadeCanvas(CanvasGroup canvas, FadeLevel fadeLevel, FadeSpeed fadeSpeed)
    {
        canvas.DOFade((int)fadeLevel, DetermineFadeTime(fadeSpeed));
    }

    // Determine fadeTime from FadeSpeed.
    private static float DetermineFadeTime(FadeSpeed speed)
    {
        switch (speed)
        {
            case FadeSpeed.Slow:
                return 5.0f;

            case FadeSpeed.Medium:
                return 3.0f;

            case FadeSpeed.Fast:
                return 1.0f;

            case FadeSpeed.SuperFast:
                return 0.5f;

            default:
                return 1.0f;
        }
    }
}
