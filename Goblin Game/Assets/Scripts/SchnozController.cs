using UnityEngine;
using System.Collections;

public class SchnozController : MonoBehaviour
{

    [Header("Detection Settings")]
    [SerializeField] private string interestTag = "objective";
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float scanFrequency = 1.0f;

    [Header("Targetting + Fine Tuning Settings")]
    [SerializeField] private Vector3 detectionOffset = Vector3.zero;
    [SerializeField] private float resetRotationDuration = 1.0f;

    private Transform target;
    private Quaternion initialRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialRotation = transform.localRotation;
        InvokeRepeating("ScanForTargets", 0f, scanFrequency);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.LookAt(target.position);
        }
    }

    private void ScanForTargets()
    {
        Transform nextTarget = GetNearestTaggedObject();

        if (target != null && nextTarget == null)
        {
            StartCoroutine(ResetSchnozRotationCoroutine(resetRotationDuration));
        }

        target = nextTarget;
    }

    private Transform GetNearestTaggedObject()
    {
        Transform nearestTarget = null;
        float nearestDistanceSqr = detectionRadius * detectionRadius;

        // This could be stupid because we're looking for all of the objects in the scene with the specified tag every scan,
        // instead of just checking the objects that are in the radius that we define. 
        // We could, later down the line, just check the objects that are in the detection radius FIRST, then filter by tag.
        foreach (GameObject candidate in GameObject.FindGameObjectsWithTag(interestTag))
        {
            float distanceSqr = (candidate.transform.position - transform.position).sqrMagnitude;

            if (distanceSqr <= nearestDistanceSqr)
            {
                nearestDistanceSqr = distanceSqr;
                nearestTarget = candidate.transform;
            }
        }

        return nearestTarget;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    private IEnumerator ResetSchnozRotationCoroutine(float duration)
    {
        Quaternion startRotation = transform.localRotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localRotation = Quaternion.Slerp(startRotation, initialRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = initialRotation;
    }
}
