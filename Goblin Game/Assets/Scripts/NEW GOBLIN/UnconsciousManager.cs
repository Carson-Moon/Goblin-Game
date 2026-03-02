using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class UnconsciousManager : NetworkBehaviour
{
    [SerializeField] List<string> impactMessages = new();

    [SerializeField] Transform ragdollPosition;
    [SerializeField] CinemachineCamera unconsciousCamera;
    [SerializeField] Camera armOverlayCamera;
    [SerializeField] CanvasGroup onHitOverlay;
    [SerializeField] TextMeshProUGUI impactText;

    [SerializeField] GameObject thirdPersonGoblin;
    [SerializeField] GoblinController goblinController;
    private GoblinRagdoll _currentRagdoll = null;


    [SerializeField] bool isUnconscious = false;

    public const string UNCONSCIOUS_LOCK = "unconscious_lock";


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
            goblinController.AddMovementLock(UNCONSCIOUS_LOCK);
            goblinController.AddLookLock(UNCONSCIOUS_LOCK);

            impactText.text = impactMessages[Random.Range(0, impactMessages.Count)];
            onHitOverlay.alpha = 1;

            armOverlayCamera.enabled = false;
            unconsciousCamera.Priority = 100;

            LoseConsciounessServerRpc(impactPoint);
        });

        sequence.Append(onHitOverlay.DOFade(0, .4f));
        sequence.AppendInterval(3f);

        sequence.AppendCallback(() =>
        {
            RegainConsciousnessServerRpc();

            goblinController.RemoveMovementLock(UNCONSCIOUS_LOCK);
            goblinController.RemoveLookLock(UNCONSCIOUS_LOCK);

            unconsciousCamera.Priority = -1;

            armOverlayCamera.enabled = true;

            isUnconscious = false;
        });
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void LoseConsciounessServerRpc(Vector3 impactPoint)
    {
        LoseConsciousnessClientRpc(impactPoint);
    }

    [ClientRpc]
    private void LoseConsciousnessClientRpc(Vector3 impactPoint)
    {
        thirdPersonGoblin.SetActive(false);

        _currentRagdoll = RagdollPool.Instance.GetRagdoll(ragdollPosition, impactPoint);
    }

    [ServerRpc(RequireOwnership = false)]
    private void RegainConsciousnessServerRpc()
    {
        RegainConsciousnessClientRpc();
    }

    [ClientRpc]
    private void RegainConsciousnessClientRpc()
    {
        thirdPersonGoblin.SetActive(true);

        if(_currentRagdoll != null)
        {
            RagdollPool.Instance.ReturnRagdoll(_currentRagdoll);
            _currentRagdoll = null;
        }
    }
}
