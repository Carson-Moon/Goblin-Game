using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class UnconsciousManager : NetworkBehaviour
{
    [SerializeField] List<string> impactMessages = new();

    [SerializeField] Transform graphicsTransform;
    [SerializeField] CinemachineCamera mainPlayerCamera;
    [SerializeField] CinemachineCamera unconsciousCamera;
    [SerializeField] Camera armOverlayCamera;
    [SerializeField] CanvasGroup onHitOverlay;
    [SerializeField] TextMeshProUGUI impactText;

    [SerializeField] GoblinCharacter goblinCharacter;


    [SerializeField] bool isUnconscious = false;
    public bool IsUnconscious => isUnconscious;


    public bool debug = false;

    void Update()
    {
        if (debug)
        {
            GetKnockedOut();
            debug = false;
        }
    }

    public void GetKnockedOut()
    {
        if (isUnconscious)
            return;

        if(!IsOwner)
            return;

        isUnconscious = true;

        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            goblinCharacter.ToggleMovement(false);
            goblinCharacter.ToggleLook(false);

            //impactText.text = impactMessages[Random.Range(0, impactMessages.Count)];

            onHitOverlay.alpha = 1;
            armOverlayCamera.enabled = false;
            unconsciousCamera.Priority = 100;

            graphicsTransform.gameObject.SetActive(true);
            graphicsTransform.localEulerAngles = new Vector3(graphicsTransform.localEulerAngles.x, graphicsTransform.localEulerAngles.y, 90);
        });
        sequence.Append(onHitOverlay.DOFade(0, .5f));
        sequence.AppendInterval(3f);
        sequence.AppendCallback(() =>
        {
            goblinCharacter.ToggleMovement(true);
            goblinCharacter.ToggleLook(true);

            armOverlayCamera.enabled = false;
            unconsciousCamera.Priority = -1;

            armOverlayCamera.enabled = true;

            graphicsTransform.gameObject.SetActive(false);
            graphicsTransform.localEulerAngles = new Vector3(graphicsTransform.localEulerAngles.x, graphicsTransform.localEulerAngles.y, 0);

            isUnconscious = false;
        });
    }


}
