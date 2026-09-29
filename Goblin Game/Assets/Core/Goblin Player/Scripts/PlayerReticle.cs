using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReticle : MonoBehaviour
{
    [SerializeField] Image reticle;
    [SerializeField] float flashDuration;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite flashSprite;
    [SerializeField] Color defaultColor;
    [SerializeField] Color flashColor;

    private Coroutine flashCoroutine = null;


    public void Flash()
    {
        if(flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }

        flashCoroutine = StartCoroutine(PerformFlash());
    }

    IEnumerator PerformFlash()
    {
        float time = 0;
        
        reticle.sprite = flashSprite;
        reticle.color = flashColor;
        while(time < flashDuration)
        {
            reticle.color = Color.Lerp(flashColor, defaultColor, time / flashDuration);
            time += Time.deltaTime;
            yield return null;
        }
        reticle.color = defaultColor;
        reticle.sprite = defaultSprite;
    }
}
