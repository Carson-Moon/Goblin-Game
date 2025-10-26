using DG.Tweening;
using UnityEngine;

public class VacuumVFX : MonoBehaviour
{
    [SerializeField] Transform rootBone;
    [SerializeField] GameObject graphicsObj;

    [SerializeField] Transform rootBoneTarget;
    [SerializeField] Transform graphicsTransform;


    void Update()
    {
        //UpdateRootBonePosition();
    }

    public void UpdateRootBonePosition()
    {
        rootBone.position = rootBoneTarget.position;
        rootBone.rotation = rootBoneTarget.rotation;
    }

    public void EnableTornado()
    {
        graphicsObj.SetActive(true);
        //graphicsTransform.DOScale(new Vector3(60, 60, 60), 1f);
    }

    public void DisableTornado()
    {
        // graphicsTransform.DOScale(new Vector3(60, 60, -1), 1f).OnComplete
        // (
        //     () => graphicsObj.SetActive(false)
        // );

        graphicsObj.SetActive(false);
    }
}
