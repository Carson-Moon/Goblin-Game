using UnityEngine;

public static class StaticPlayerPrefsHelper
{
    private static string usernamePref = "username";
    public static string UsernamePref => usernamePref;

    private static string defaultUsername = "mindgob";
    public static string DefaultUsername => defaultUsername;

    private static Color defaultFaceColorShift = Color.white;
    public static Color colorShiftDefaultFaceColor => defaultFaceColorShift;
    private static Color defaultBodyColorShift = Color.white;
    public static Color colorShiftDefaultBodyColor => defaultBodyColorShift;
}
