using TMPro;
using UnityEngine;

// Responsible for controlling our loading screen.

public class LoadingScreenManager : MonoBehaviour
{
    // Singleton
    public static LoadingScreenManager instance { get; private set; }

    [Header("Loading Screen Elements")]
    [SerializeField] CanvasGroup loadingGroup;
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] TextMeshProUGUI splashText;

    [Header("Settings")]
    [SerializeField] string[] splashTextOptions;


    // Enable our loading screen.
    public void EnableLoadingScreen()
    {
        mainText.text = "Loading. . .";

        // Set our splash text.
        splashText.text = splashTextOptions[Random.Range(0, splashTextOptions.Length)];

        // Fade our canvas in.
        CanvasFader.FadeCanvas(loadingGroup, FadeLevel.FullyOpaque, FadeSpeed.Medium);
    }

    public void EnableLoadingScreen(string bigText)
    {
        mainText.text = bigText;

        // Set our splash text.
        splashText.text = splashTextOptions[Random.Range(0, splashTextOptions.Length)];

        // Fade our canvas in.
        CanvasFader.FadeCanvas(loadingGroup, FadeLevel.FullyOpaque, FadeSpeed.Medium);
    }

    // Disable our loading screen.
    public void DisableLoadingScreen()
    {
        // Fade our canvas in.
        CanvasFader.FadeCanvas(loadingGroup, FadeLevel.FullyTransparent, FadeSpeed.Medium);
    }
}
