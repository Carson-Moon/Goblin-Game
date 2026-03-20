using DG.Tweening;
using UnityEngine;

public class DamageVignette : MonoBehaviour
{
    [SerializeField] CanvasGroup vignetteGroup;

    private Sequence flash = null;


    void Awake()
    {
        vignetteGroup.TurnOff();
    }

    public void PerformDamageFlash()
    {
        Debug.Log("Damage Flash!");

        flash.Kill();
        flash = DOTween.Sequence();

        flash.Append(vignetteGroup.DOFade(1 , .1f));
        flash.Append(vignetteGroup.DOFade(0, .3f));
    }
}
