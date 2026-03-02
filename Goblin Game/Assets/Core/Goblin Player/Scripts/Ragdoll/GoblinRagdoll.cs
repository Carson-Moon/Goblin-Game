using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoblinRagdoll : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float forceStrength;
    private List<RagdollRigidbody> ragdollRbs = new();

    private IEnumerable<Rigidbody> orderedRigidbodies;
    private Vector3 forceDirection;


    void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        var rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach(var rb in rigidbodies)
        {
            RagdollRigidbody ragdollRb = new(rb, rb.transform, rb.transform.localPosition, rb.transform.localRotation);
            ragdollRbs.Add(ragdollRb);
        }

        gameObject.SetActive(false);
    }

    public void Ragdoll(Vector3 spawnPoint, Quaternion spawnRotation, Vector3 impactPoint)
    {
        transform.position = spawnPoint;
        transform.rotation = spawnRotation;
        gameObject.SetActive(true);

        // Apply forces to the closest rigidbodies to the impact point.
        orderedRigidbodies = ragdollRbs.OrderBy(x => Vector3.Distance(x.Rb.position, impactPoint)).Select(x => x.Rb).Take(4);

        foreach(var rb in orderedRigidbodies)
        {
            forceDirection = (rb.position - impactPoint).normalized;
            rb.AddForce(forceDirection * forceStrength, ForceMode.Impulse);
        }
    }

    public void ResetRagdoll()
    {
        gameObject.SetActive(false);

        foreach(var ragdollRb in ragdollRbs)
        {
            ragdollRb.ResetTransform();
        }
    }
}
