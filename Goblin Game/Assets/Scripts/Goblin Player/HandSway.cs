using UnityEngine;

public class HandSway : MonoBehaviour
{
    [SerializeField] Transform target;

    [Header("Position")]
    [SerializeField] Vector3 localOffset;
    [SerializeField] float positionResponse = 20f;

    [Header("Rotation Spring")]
    [SerializeField] float frequency = 2.5f;
    [Range(0f, 1f), SerializeField] float dampingRatio = 0.6f;
    [SerializeField] float kickPerDegree = 0.35f;
    [SerializeField] float maxAngleOffset = 8f;

    // Spring state: x = pitch, y = yaw, z = roll, in degrees offset from the camera.
    Vector3 _swing;
    Vector3 _swingVelocity;

    Quaternion _lastTargetRotation;

    public void Initialize()
    {
        _lastTargetRotation = target.rotation;
        transform.SetPositionAndRotation(target.position, target.rotation);
    }

    public void Tick(float deltaTime)
    {
        Debug.Log("arms tick");
        var dt = Mathf.Min(deltaTime, 1f / 30f);

        // How much the camera turned since last frame, in camera-local axes.
        var delta = Quaternion.Inverse(_lastTargetRotation) * target.rotation;
        delta.ToAngleAxis(out var angle, out var axis);
        if (angle > 180f) angle -= 360f;
        var localDelta = axis * angle;

        _lastTargetRotation = target.rotation;

        // Camera turning kicks the spring the opposite way — the arms get left behind.
        _swingVelocity -= localDelta * kickPerDegree / Mathf.Max(dt, 0.0001f) * dt;

        var omega = 2f * Mathf.PI * frequency;
        _swingVelocity += (-omega * omega * _swing - 2f * dampingRatio * omega * _swingVelocity) * dt;
        _swing += _swingVelocity * dt;

        if (_swing.sqrMagnitude < 1e-8f && _swingVelocity.sqrMagnitude < 1e-8f)
        {
            _swing = Vector3.zero;
            _swingVelocity = Vector3.zero;
        }

        var clamped = Vector3.ClampMagnitude(_swing, maxAngleOffset);

        transform.rotation = target.rotation * Quaternion.Euler(clamped);

        var posT = 1f - Mathf.Exp(-positionResponse * dt);
        transform.position = Vector3.Lerp(transform.position,
            target.position + target.TransformDirection(localOffset), posT);
    }
}