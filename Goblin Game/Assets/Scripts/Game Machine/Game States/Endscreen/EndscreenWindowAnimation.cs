using UnityEngine;
using DG.Tweening;

// This is how the endscreen enters the frame.

public class EndscreenWindowAnimation : Abstract_Canvas_Animation
{
    [Header("Settings")]
    [SerializeField] float translationSeconds;


    public override void PlayAnimation()
    {
        windowTransform.DOMove(targetTransform.position, translationSeconds).SetEase(Ease.OutElastic);
    }
}
