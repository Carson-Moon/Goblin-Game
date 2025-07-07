using UnityEngine;
using UnityEngine.Splines;

public class FlythroughData : MonoBehaviour
{
    [SerializeField] SplineContainer spline;
    [SerializeField] Transform lookAtTarget;
    [SerializeField] float speed = 5f;

    public SplineContainer GetSpline()
    {
        return spline;
    }

    public Transform GetLookAtTarget()
    {
        return lookAtTarget;
    }

    public float GetSpeed()
    {
        return speed;
    }
}
