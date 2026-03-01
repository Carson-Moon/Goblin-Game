using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using DG.Tweening;
using Unity.Netcode;

public class FlythroughState : GameState
{
    [SerializeField] List<FlythroughData> flythroughDatas = new();
    [SerializeField] CinemachineCamera flythroughCam;
    [SerializeField] CinemachineSplineDolly flythroughDolly;


    protected override void OnStartStateServer()
    {
        CreateLock();

        OnStartStateClientRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    public override void OnStartStateClientRpc()
    {
        GoblinClientPointer.LocalGoblinClient().ArmOverlayCamera.enabled = false;

        LoadingScreenManager.Instance.DisableLoadingScreen();

        PerformFlythrough(0);
    }

    private void PerformFlythrough(int index, bool lastSpline = false)
    {
        FlythroughData thisData = flythroughDatas[index];

        // Attach and reset our camera.
        flythroughDolly.Spline = thisData.Spline;
        flythroughDolly.CameraPosition = 0;
        flythroughCam.LookAt = thisData.LookAtTarget;
        flythroughCam.Priority = 100;

        DOTween.To(() => flythroughDolly.CameraPosition, x => flythroughDolly.CameraPosition = x, 1f, thisData.Speed)
            .SetEase(Ease.Linear)
            .OnUpdate
            (
                () =>
                {
                    if (lastSpline && !LoadingScreenManager.Instance.IsEnabled && flythroughDolly.CameraPosition > 0.3f)
                    {
                        LoadingScreenManager.Instance.EnableLoadingScreen();
                    }
                        
                }
            )
            .OnComplete
            (
                () =>
                {
                    if (index == flythroughDatas.Count - 2)
                        PerformFlythrough(index + 1, true);
                    else if (index < flythroughDatas.Count - 1)
                        PerformFlythrough(index + 1);
                    else
                    {
                        GoblinClientPointer.LocalGoblinClient().ArmOverlayCamera.enabled = true;
                        flythroughCam.Priority = -1;

                        LoadingScreenManager.Instance.DisableLoadingScreen();

                        ClientUnlock();
                    }
                }
            );
    }
}
