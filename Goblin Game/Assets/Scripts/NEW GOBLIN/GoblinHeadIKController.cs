using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// I believe the only thing we should have to network is the targetLookPosition?
// Everything else can be local and is used to DETERMINE that position.
// Maybe the best thing to do is send just an interest point over the network?

// If its not too expensive, just run all players IKs locally?      CURRENTLY DOING THIS.

public class GoblinHeadIKController : MonoBehaviour
{
    [Header("IK Settings")]
    [SerializeField] Transform headIKTarget;
    [SerializeField] Transform defaultTarget;
    [SerializeField] Vector3 targetLookPosition;
    [SerializeField] float targetSmoothing;
    private Vector3 headVel;

    [Header("Interest Point Detection")]
    [SerializeField] List<InterestPoint> interestPoints = new();
    [SerializeField] InterestPoint myInterestPoint;
    [SerializeField] float detectionInterval;
    [SerializeField] float detectionRadius;
    [SerializeField] LayerMask detectionLayers;
    private float detectionCooldown;
    private Collider[] detectedColliders = { };


    void Awake()
    {
        detectionCooldown = detectionInterval;
    }

    void Update()
    {
        // Cooldown.
        detectionCooldown -= Time.deltaTime;
        if (detectionCooldown < 0)
        {
            detectionCooldown = detectionInterval;
            DetectInterestPoints();
        }

        targetLookPosition = DetermineBestLookPosition();

        // Update target position!
        headIKTarget.position = Vector3.SmoothDamp(headIKTarget.position, targetLookPosition, ref headVel, targetSmoothing);
    }

    private Vector3 DetermineBestLookPosition()
    {
        InterestPoint mostInterestingPoint = null;
        foreach (InterestPoint ip in interestPoints)
        {
            if (mostInterestingPoint == null)
            {
                mostInterestingPoint = ip;
                continue;
            }

            // If this new point has lower interest, switch to this one.
            if (ip.GetInterest(transform.position) < mostInterestingPoint.GetInterest(transform.position))
            {
                mostInterestingPoint = ip;
            }
        }

        if (mostInterestingPoint == null)
        {
            return defaultTarget.position;
        }
        else
        {
            return mostInterestingPoint.Position;
        }
    }

    private void DetectInterestPoints()
    {
        interestPoints.Clear();

        detectedColliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayers);

        if (detectedColliders.Length > 0)
        {
            foreach (Collider col in detectedColliders)
            {
                if (col.TryGetComponent<InterestPoint>(out InterestPoint ip))
                {
                    if (ip == myInterestPoint)
                        continue;
                    
                    interestPoints.Add(ip);
                }
            }
        }
    }
}
