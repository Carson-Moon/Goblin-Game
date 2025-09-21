using System;
using TMPro;
using UnityEngine;

// Responsible for controlling our loading screen.

public class LoadingScreenManager : MonoBehaviour
{
    // Singleton
    public static LoadingScreenManager Instance { get; private set; }

    private bool isEnabled = false;
    public bool IsEnabled => isEnabled;

    [Header("Loading Screen Elements")]
    [SerializeField] CanvasGroup loadingGroup;
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] TextMeshProUGUI splashText;

    [Header("Settings")]
    [SerializeField] string[] splashTextOptions;


    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        DontDestroyOnLoad(this);
    }

    // Enable our loading screen.
    public void EnableLoadingScreen(string overrideMainText = "", string overrideSplashText = "", Action onFadeComplete = null)
    {
        isEnabled = true;

        // Set our main text.
        if (string.Compare(overrideMainText, "") == 0)
            mainText.text = "Loading. . .";
        else
            mainText.text = overrideMainText;

        // Set our splash text.
        if (string.Compare(overrideSplashText, "") == 0)
            splashText.text = splashTextOptions[UnityEngine.Random.Range(0, splashTextOptions.Length)];
        else
            splashText.text = overrideSplashText;

        // Fade our canvas in.
        CanvasFader.FadeCanvas(loadingGroup, FadeLevel.FullyOpaque, FadeSpeed.Medium, onFadeComplete);
    }

    // Disable our loading screen.
    public void DisableLoadingScreen(Action onFadeComplete = null)
    {
        isEnabled = false;

        // Fade our canvas in.
        CanvasFader.FadeCanvas(loadingGroup, FadeLevel.FullyTransparent, FadeSpeed.Medium, onFadeComplete);
    }
}
