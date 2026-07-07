using UnityEngine;

public static class ColorHolder
{
    private static float colorR;
    private static float colorG;
    private static float colorB;

    public static Color Color => new Color(colorR, colorG, colorB);


    private const string COLOR_PLAYER_PREF_R = "colorR";
    private const string COLOR_PLAYER_PREF_G = "colorG";
    private const string COLOR_PLAYER_PREF_B = "colorB";


    public static void FetchExistingColor()
    {
        colorR = PlayerPrefs.GetFloat(COLOR_PLAYER_PREF_R, 0);
        colorG = PlayerPrefs.GetFloat(COLOR_PLAYER_PREF_G, 1);
        colorB = PlayerPrefs.GetFloat(COLOR_PLAYER_PREF_B, 0);
    }

    public static void SetNewColor(Color newColor)
    {
        Debug.Log($"Set new color: {newColor}");

        PlayerPrefs.SetFloat(COLOR_PLAYER_PREF_R, newColor.r);
        PlayerPrefs.SetFloat(COLOR_PLAYER_PREF_G, newColor.b);
        PlayerPrefs.SetFloat(COLOR_PLAYER_PREF_B, newColor.b);
        PlayerPrefs.Save();

        FetchExistingColor();
    }
}
