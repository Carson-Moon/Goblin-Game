using UnityEngine;
using DG.Tweening;

public class Window_Spring : Abstract_Toggle_Window
{
    [Header("Spring Settings")]
    [SerializeField] RectTransform window;
    [SerializeField] float springSpeed;


    void Start()
    {
        if (startHidden)
        {
            window.localScale = new Vector3(0, 1, 1);
            isHidden = true;
        }
        else
        {
            window.localScale = Vector3.one;
            isHidden = false;
        }
    }

    public override void ToggleWindow()
    {
        isHidden = !isHidden;
        if (isHidden)
            CloseWindow();
        else
            OpenWindow();
    }

    public override void OpenWindow()
    {
        window.DOScale(Vector3.one, springSpeed).SetEase(Ease.OutElastic);
    }

    public override void CloseWindow()
    {
        window.DOScale(new Vector3(0, 1, 1), springSpeed).SetEase(Ease.InElastic);
    }
}
