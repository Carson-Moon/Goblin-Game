using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(Vector3 damagePoint);
    public void OnDeath();
}
