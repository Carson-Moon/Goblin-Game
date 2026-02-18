using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class UnconsciousManager : NetworkBehaviour
{
    [SerializeField] List<string> impactMessages = new();

    [SerializeField] CinemachineCamera unconsciousCamera;
    [SerializeField] Camera armOverlayCamera;
    [SerializeField] CanvasGroup onHitOverlay;
    [SerializeField] TextMeshProUGUI impactText;

    [SerializeField] GoblinCharacter goblinCharacter;
    [SerializeField] GoblinRagdoll goblinRagdoll;


    [SerializeField] bool isUnconscious = false;


    public bool debug = false;

    void Update()
    {
        if (debug)
        {
            LoseConsciousness(Vector3.zero);
            debug = false;
        }
    }

    public void LoseConsciousness(Vector3 impactPoint)
    {
        if(isUnconscious)
        {
            Debug.Log("Already knocked out!");
            return;
        }
            
        isUnconscious = true;

        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            goblinCharacter.ToggleMovement(false);
            goblinCharacter.ToggleLook(false);

            impactText.text = impactMessages[Random.Range(0, impactMessages.Count)];
            onHitOverlay.alpha = 1;

            armOverlayCamera.enabled = false;
            unconsciousCamera.Priority = 100;

            LoseConsciousnessClientRpc(impactPoint);
        });

        sequence.Append(onHitOverlay.DOFade(0, .4f));
        sequence.AppendInterval(3f);

        sequence.AppendCallback(() =>
        {
            RegainConsciousnessClientRpc();

            goblinCharacter.ToggleMovement(true);
            goblinCharacter.ToggleLook(true);

            unconsciousCamera.Priority = -1;

            armOverlayCamera.enabled = true;

            isUnconscious = false;
        });
        
    }

    [ClientRpc]
    private void LoseConsciousnessClientRpc(Vector3 impactPoint)
    {
        goblinRagdoll.Ragdoll(impactPoint);
    }

    [ClientRpc]
    private void RegainConsciousnessClientRpc()
    {
        goblinRagdoll.ResetRagdoll();
    }
}
