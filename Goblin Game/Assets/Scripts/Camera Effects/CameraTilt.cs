using UnityEngine;

/// <summary>
///  Camera tilt effect based on the character's lateral movement.
///  To be added underneath the Camera Effects Stack 
/// </summary>
public class CameraTilt : CameraEffect
{
    [SerializeField] private GoblinCharacter _character;

    [SerializeField] private float _maxTilt = 2.5f;
    [SerializeField] private float _referenceSpeed = 8f;
    [SerializeField] private float _responsiveness = 10f;

    private float _tilt;

    public override Vector3 EulerOffset => new Vector3(0f, 0f, _tilt);

    public override void Tick(float deltaTime)
    {
        var motor = _character.getKinematicCharacterMotor();

        Vector3 planar = Vector3.ProjectOnPlane(motor.Velocity, motor.CharacterUp);
        float lateral = Vector3.Dot(planar, motor.CharacterRight);

        float target = Mathf.Clamp(-lateral / _referenceSpeed, -1f, 1f) * _maxTilt;

        _tilt = Mathf.Lerp(_tilt, target, 1f - Mathf.Exp(-_responsiveness * deltaTime));
    }
}