using UnityEngine;

public abstract class Abstract_Canvas_Animation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] protected RectTransform windowTransform;
    [SerializeField] protected RectTransform targetTransform;


    public virtual void PlayAnimation()
    {
        
    }
}
