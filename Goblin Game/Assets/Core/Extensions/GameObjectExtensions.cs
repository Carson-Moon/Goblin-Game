using UnityEngine;
using DG.Tweening;

public static class GameObjectExtensions
{
#region Canvas Group
    public static void TurnOn(this CanvasGroup group, float duration = 0)
    {
        group.DOFade(1, duration);
        group.blocksRaycasts = true;
    }

    public static void TurnOff(this CanvasGroup group, float duration = 0)
    { 
        group.DOFade(0, duration);
        group.blocksRaycasts = false;
    }
#endregion
}
