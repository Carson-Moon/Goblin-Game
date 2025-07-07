using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

// THIS STATE IS WHERE OUR CAMERA INTRODUCES AND FLYS THROUGH THE MAP.

public class CameraIntroState : GameState
{
    [Header("Flythrough Settings")]
    [SerializeField] List<FlythroughData> flythroughData = new List<FlythroughData>();
    [SerializeField] CinemachineCamera flythroughCam;
    [SerializeField] CinemachineSplineDolly flythroughDolly;
    [SerializeField] Client_Goblin clientGoblin;


    public override void StartThisState()
    {
        base.StartThisState();

        // Grab our client goblin script and disable our overlay camera for the flythrough.
        clientGoblin = ConnectedPlayerManager.instance.GetClientGoblin(NetworkManager.Singleton.LocalClientId);
        clientGoblin.SetOverlayCamera(false);
        clientGoblin.SetMovement(false);

        StartCoroutine(PerformFlythrough(0));
    }

    // Performs a flythrough of 1 spline. Loops if we have more splines to go!
    IEnumerator PerformFlythrough(int index)
    {
        FlythroughData thisData = flythroughData[index];

        // Attach and reset our camera.
        flythroughDolly.Spline = thisData.GetSpline();
        flythroughDolly.CameraPosition = 0;
        flythroughCam.LookAt = thisData.GetLookAtTarget();
        flythroughCam.Priority = 100;

        float time = 0;
        bool startedFade = false;

        FadeUI.StartFade(0, 0.5f);
        while (time < thisData.GetSpeed())
        {
            // Fade out if we have half a second left.
            if (!startedFade && thisData.GetSpeed() - time <= 0.5f)
            {
                startedFade = true;
                FadeUI.StartFade(1, 0.5f);
            }

            flythroughDolly.CameraPosition = Mathf.Lerp(0, 1, time / thisData.GetSpeed());

            time += Time.deltaTime;
            yield return null;
        }
        flythroughDolly.CameraPosition = 1;

        // Recursive if we have another spline to follow.
        if (index < flythroughData.Count - 1)
        {
            StartCoroutine(PerformFlythrough(index + 1));
        }
        else
        {
            yield return new WaitForSeconds(2f);

            // Make sure our flythrough camera has no priority anymore.
            flythroughCam.Priority = -1;

            EndThisState();
        }

    }

    public override void EndThisState()
    {
        base.EndThisState();
    }
}
