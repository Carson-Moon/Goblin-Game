using System;
using UnityEngine;

[Serializable]
public class RagdollRigidbody
{
    public Rigidbody Rb;
    public Transform Transform;
    public Vector3 StartingPosition;
    public Quaternion StartingRotation;

    
    public RagdollRigidbody(Rigidbody _rigidBody, Transform _transform, Vector3 _startingPosition, Quaternion _startingRotation)
    {
        Rb = _rigidBody;
        Transform = _transform;
        StartingPosition = _startingPosition;
        StartingRotation = _startingRotation;
    }

    public void ResetTransform()
    {
        Transform.localPosition = StartingPosition;
        Transform.localRotation = StartingRotation;
    }
}
