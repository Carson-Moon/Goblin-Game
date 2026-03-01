using UnityEngine;

public class HandSway : MonoBehaviour
{
    [Header("Sway Settings")]
    public float swayAmount = 0.05f;
    public float maxSwayAmount = 0.1f;
    public float smoothSpeed = 8f;

    [Header("Rotation Sway Settings")]
    public float rotationAmount = 5f;
    public float tiltAmount = 5f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        // Determine the target offset based on the negative direction that the mouse moves.
        float targetX = Mathf.Clamp(-mouseX * swayAmount, -maxSwayAmount, maxSwayAmount);
        float targetY = Mathf.Clamp(-mouseY * swayAmount, -maxSwayAmount, maxSwayAmount);

        Vector3 targetPosition = new Vector3(targetX, targetY, 0) + initialPosition;

        // lerp towards target (the initial position plus the target offset)
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * smoothSpeed);

        // rotation sway
        Quaternion targetRotation = Quaternion.Euler(-mouseY * rotationAmount, mouseX * rotationAmount, mouseX * tiltAmount);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smoothSpeed);
    }
}
