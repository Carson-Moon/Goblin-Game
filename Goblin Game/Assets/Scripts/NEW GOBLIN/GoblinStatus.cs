using UnityEngine;

// Used to hold and perform any status effects.
// Other goblin scripts look here to see if we are currently affected by anything.

public class GoblinStatus : MonoBehaviour
{
    [Header("Current Status")]
    [SerializeField] StatusEffects currentStatus = StatusEffects.None;
    public StatusEffects CurrentStatus => currentStatus;
}
