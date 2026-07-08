using UnityEngine;

public static class UsernameHolder
{
    private static string username = string.Empty;
    public static string Username => username;


    private const string USERNAME_PLAYER_PREF = "username";


    public static string FetchExistingUsername()
    {
        username = PlayerPrefs.GetString(USERNAME_PLAYER_PREF, "Mind Goblin");
        return username;
    }

    public static void SetNewUsername(string newUsername)
    {
        PlayerPrefs.SetString(USERNAME_PLAYER_PREF, newUsername);
        PlayerPrefs.Save();

        username = newUsername;
    }

    public static bool HasValidUsername()
    {
        return !string.IsNullOrEmpty(username);
    }
}
