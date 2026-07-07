using UnityEngine;

public static class LocalPlayerProfile
{
    private static PlayerProfile profile;
    public static PlayerProfile Profile => profile;


    [Tooltip("Returns if profile was successfully setup.")]
    public static bool SetProfile(string displayName, Color goblinColor)
    {
        if(string.Compare(displayName, string.Empty) == 0)
            return false;

        profile = new PlayerProfile()
        {
            DisplayName = displayName,
            GoblinColor = goblinColor  
        };
        return true;
    }

    public static bool IsProfileEmpty()
    {
        return profile.Equals(default(PlayerProfile));
    }
}
