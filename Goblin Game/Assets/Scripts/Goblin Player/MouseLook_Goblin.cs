using UnityEngine;

public class MouseLook_Goblin : MonoBehaviour
{
    // Runtime
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _mainCamera;


    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        _orientation.forward = new Vector3(_mainCamera.forward.x, 0.0f, _mainCamera.forward.z);
    }
}
