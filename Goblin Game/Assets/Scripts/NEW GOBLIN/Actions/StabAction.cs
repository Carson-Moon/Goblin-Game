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
    [SerializeField] Animator anim;
    private int AttackHash = Animator.StringToHash("attack");


    public void AttemptStab()
    {
        if (onCooldown)
            return;

        PerformStab();
    }

    public void PerformStab()
    {
        if (anim) anim.SetTrigger(AttackHash);
        if (goblinAnimator) goblinAnimator.StabAnimation();

        Collider[] cols = Physics.OverlapSphere(stabPosition.position, stabRadius, stabbableMask);
        foreach (Collider col in cols)
        {
            //Debug.Log($"Collider! {col.gameObject.name}");
            IDamageable damageable = col.GetComponent<IDamageable>();
            if (damageable == null)
                continue;

            //Debug.Log("Damageable!");
            damageable.TakeDamage(stabPosition.position);
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
