using UnityEngine;
using UnityEngine.Events;

public class DummyStabAction : MonoBehaviour, IDamageable
{
    [SerializeField] UnityEvent onStabEvent;

    public void OnDeath()
    {

    }

    public void TakeDamage()
    {
        Debug.Log("Perform action!");
        onStabEvent?.Invoke();
    }
}
