using UnityEngine;

public class InterestPoint : MonoBehaviour
{
    public Vector3 Position => transform.position;
    [SerializeField][
    Tooltip("Lower values, more likely to look at.")]
    [Range(1, 100)]
    int interestPower = 1;


    // Interest is calculated by the distance away from the head controller 
    // multiplied by the interestPower of this point.
    public float GetInterest(Vector3 pos)
    {
        return (Position - pos).sqrMagnitude * interestPower;
    }
}
