using UnityEngine;

public struct CameraInput
{
    public Vector2 Look;
}

public class GoblinCamera : MonoBehaviour
{
    [SerializeField] float sensitivity = 0.1f;

    private Vector3 _eulerAngles;

    public void Initialize(Transform cameraTarget)
    {
        Cursor.lockState = CursorLockMode.Locked;

        transform.position = cameraTarget.position;
        transform.eulerAngles = _eulerAngles = cameraTarget.eulerAngles;
    }

    public void UpdateRotation(CameraInput input)
    {
        _eulerAngles += new Vector3(-input.Look.y, input.Look.x) * sensitivity;
        transform.eulerAngles = _eulerAngles;
    }

    public void UpdatePosition(Transform target)
    {
        transform.position = target.position;
    }
}
