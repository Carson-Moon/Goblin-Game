using UnityEngine;
using Unity.Cinemachine;

public struct CameraInput
{
    public Vector2 Look;
}

public class GoblinCamera : MonoBehaviour
{
    [SerializeField] float sensitivity = 0.1f;
    [SerializeField] float verticalClamp;
    [SerializeField] Unity.Cinemachine.CinemachineCamera _camera;
    [SerializeField] Camera _armCamera;
    [SerializeField, Range(0f, 1f)] float _armFovRatio = 0f; 
    private Vector3 _eulerAngles;
    private float _baseFov;

    public void Initialize(Transform cameraTarget)
    {
        // Cursor.lockState = CursorLockMode.Locked;

        transform.position = cameraTarget.position;
        transform.eulerAngles = _eulerAngles = cameraTarget.eulerAngles;
        _baseFov = _camera.Lens.FieldOfView;
    }

    public void UpdateRotation(CameraInput input, Vector3 eulerOffset)
    {
        _eulerAngles += new Vector3(-input.Look.y, input.Look.x) * sensitivity;

        _eulerAngles = new Vector3(Mathf.Clamp(_eulerAngles.x, -verticalClamp, verticalClamp), _eulerAngles.y, _eulerAngles.z);

        transform.eulerAngles = _eulerAngles + eulerOffset;
    }

    public void UpdatePosition(Transform target, Vector3 positionOffset)
    {
        transform.position = target.position + positionOffset;
    }

    public void UpdateFov(float fovOffset)
    {
        _camera.Lens.FieldOfView = _baseFov + fovOffset;
        _armCamera.fieldOfView = _baseFov * _armFovRatio + fovOffset;
    }
}
