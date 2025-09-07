using UnityEngine;

// This is the in and out of a goblin. Things will look here for information!

public class GoblinClient : MonoBehaviour
{
    [Header("Goblin Client Information")]
    [SerializeField] string goblinName;
    public string GoblinName => goblinName;
    [SerializeField] ulong goblinID;
    public ulong GoblinID => goblinID;
}
