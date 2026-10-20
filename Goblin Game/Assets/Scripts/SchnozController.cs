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
    [SerializeField] private float targetRotationDuration = 0.5f;
    [SerializeField] private float resetRotationDuration = 1.0f;
    [SerializeField] private float minimumRotationAngle = 0f;
    [SerializeField] private float maximumRotationAngle = 85f;

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

            // Calculate the direction to the target and if the targetDirection's square magnitude is less than or equal to Mathf.Epsilon, 
            // return early to avoid unnecessary calculations.
            Vector3 targetDirection = target.position - transform.position;
            if (targetDirection.sqrMagnitude <= Mathf.Epsilon)
            {
                return;
            }

            // Determine the local target direction based on whether the object has a parent or not (it will)
            Transform parent = transform.parent;

            // If the object has a parent, convert the target direction to local space using InverseTransformDirection.
            Vector3 localTargetDirection = parent != null
                ? parent.InverseTransformDirection(targetDirection)
                : targetDirection;

            // Calculate the desired local rotation using Quaternion.LookRotation, 
            // which creates a rotation that looks in the direction of localTargetDirection while keeping the 
            // up direction aligned with the initial rotation's up vector.
            Quaternion desiredLocalRotation = Quaternion.LookRotation(
                localTargetDirection,
                initialRotation * Vector3.up);

            // Calculate the target rotation by rotating towards the desired local rotation,
            // clamping the rotation angle between minimumRotationAngle and maximumRotationAngle.
            Quaternion targetRotation = Quaternion.RotateTowards(
                initialRotation,
                desiredLocalRotation,
                Mathf.Clamp(maximumRotationAngle, minimumRotationAngle, 180f));
            float rotationDuration = Mathf.Max(targetRotationDuration, Mathf.Epsilon);
            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                targetRotation,
                Time.deltaTime / rotationDuration);
        }
    }

    private void ScanForTargets()
    {
        Transform nextTarget = GetNearestTargetInRadius();

        if (target != null && nextTarget == null)
        {
            StartCoroutine(ResetSchnozRotationCoroutine(resetRotationDuration));
        }

        target = nextTarget;
    }

    private Transform GetNearestTargetInRadius()
    {
        Transform nearestTarget = null;
        float nearestDistanceSqr = detectionRadius * detectionRadius;

        foreach (Collider candidateCollider in Physics.OverlapSphere(transform.position, detectionRadius))
        {
            GameObject candidate = candidateCollider.gameObject;

            if (!candidate.CompareTag(interestTag))
            {
                continue;
            }

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
