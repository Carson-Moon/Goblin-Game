using UnityEngine;

public class Stab_Goblin : MonoBehaviour
{
    [Header("Stab Settings")]
    [SerializeField] bool canStab = true;
    [SerializeField] Transform stabPosition;
    [SerializeField] float stabRadius;
    [SerializeField] LayerMask stabMask;
    [SerializeField] float stabCooldownLength;
    [SerializeField] float stabCooldown;


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

        print("Stab!");

        // Determine if we hit anything.
        Collider[] cols = Physics.OverlapSphere(stabPosition.position, stabRadius, stabMask);

        for(int i=0; i<cols.Length; i++)
        {
            print("Stabbed " + cols[i].name);
        }

        // Reset stab cooldown.
        stabCooldown = stabCooldownLength;

        canStab = false;
    }
}
