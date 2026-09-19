using DG.Tweening;
using UnityEngine;

// Performs a stab!

public class StabAction : MonoBehaviour
{
    [Header("Stab Settings")]
    [SerializeField] Transform stabPosition;
    [SerializeField] float stabRadius;
    [SerializeField] LayerMask stabbableMask;
    [SerializeField] float stabCooldownLength;
    [SerializeField] bool onCooldown;

    [Header("Animator")]
    [SerializeField] GoblinAnimator goblinAnimator;
    [SerializeField] NetworkMovementAnimator networkAnimator;
    [SerializeField] Animator anim;
    private int AttackHash = Animator.StringToHash("attack1");


    public void AttemptStab()
    {
        if (onCooldown)
            return;

        PerformStab();
    }

    public void PerformStab()
    {
        if (anim != null) anim.SetTrigger(AttackHash);
        if (goblinAnimator != null) goblinAnimator.StabAnimation();
        if(networkAnimator != null) networkAnimator.StabAnimationClientRpc();

        Collider[] cols = Physics.OverlapSphere(stabPosition.position, stabRadius, stabbableMask);
        foreach (Collider col in cols)
        {
            //Debug.Log($"Collider! {col.gameObject.name}");
            IDamageable damageable = col.GetComponent<IDamageable>();
            if (damageable == null)
                continue;

            damageable.TakeDamage(stabPosition.position);
            RoundStatTracker.Instance.TrackIntStat(IntStat.Stabbed_Someone);
        }

        PerformCooldown();
    }

    private void PerformCooldown()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => onCooldown = true);
        sequence.AppendInterval(stabCooldownLength);
        sequence.AppendCallback(() => onCooldown = false);
    }
}
