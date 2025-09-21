using UnityEngine;
using UnityEngine.Splines;

public class FlythroughData : MonoBehaviour
{
    [SerializeField] SplineContainer spline;
    public SplineContainer Spline => spline;

    [SerializeField] Transform lookAtTarget;
    public Transform LookAtTarget => lookAtTarget;

    [SerializeField] float speed = 5f;
    public float Speed => speed;
}
