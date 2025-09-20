using UnityEngine;

public class Stab_Goblin : MonoBehaviour
{
    [Header("Stab Settings")]
    [SerializeField] bool canStab = true;
    [SerializeField] Transform stabPosition;
    [SerializeField] float stabRadius;
    [SerializeField] LayerMask stabMask;
    [SerializeField] LayerMask breakableMask;
    [SerializeField] float stabCooldownLength;
    [SerializeField] float stabCooldown;

    [Header("Animations")]
    [SerializeField] private Animator m_Anim;
    private int m_AttackHash = Animator.StringToHash("attack");


    void Update()
    {
        // Stab cooldown.
        if(!canStab)
        {
            stabCooldown -= Time.deltaTime;
            stabCooldown = Mathf.Clamp(stabCooldown, 0, stabCooldownLength);

            if(stabCooldown <= 0)
            {
                canStab = true;
            } 
        }
    }

    // Attempt to stab.
    public void OnStab()
    {
        if(!canStab)
            return;

        // Play Animation.
        m_Anim.SetTrigger(m_AttackHash);

        //print("Stab!");

        // Determine if we hit any goblins.
        Collider[] cols = Physics.OverlapSphere(stabPosition.position, stabRadius, stabMask);

        for (int i = 0; i < cols.Length; i++)
        {
            //print("Stabbed " + cols[i].name);
            cols[i].GetComponent<CoinManager_Goblin>().LoseCoin();
            RoundStatTracker.instance.TrackIntStat(IntStat.StabbedSomeone);
        }

        // Determine if we hit any breakables.
        cols = Physics.OverlapSphere(stabPosition.position, stabRadius, breakableMask);

        for(int i=0; i<cols.Length; i++)
        {
            //print("Stabbed " + cols[i].name);
            cols[i].GetComponent<Breakable>().TakeDamage(stabPosition.position);
        }

        // Reset stab cooldown.
        stabCooldown = stabCooldownLength;

        canStab = false;
    }
}
