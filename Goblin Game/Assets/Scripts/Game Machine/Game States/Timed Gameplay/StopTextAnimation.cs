using UnityEngine;
using DG.Tweening;

// Screeches to a hault!

public class StopTextAnimation : Abstract_Canvas_Animation
{
    [Header("Additional Settings")]
    [SerializeField] float translationSeconds;


    public override void PlayAnimation()
    {
        windowTransform.DOMove(targetTransform.position, translationSeconds).SetEase(Ease.OutElastic);
    }
}
