using UnityEngine;

public class JarPickup : AbstractBreakablePickup
{
    [Header("Jar Specific Settings")]
    [SerializeField] int coins;
    public int Coins => coins;


    public override void OnPickup(Transform pickupPos)
    {
        base.OnPickup(pickupPos);
    }

    public override void OnThrow(Vector3 throwDirection, float throwForce)
    {
        base.OnThrow(throwDirection, throwForce);
    }

    public override void Break()
    {
        base.Break();
    }
}
