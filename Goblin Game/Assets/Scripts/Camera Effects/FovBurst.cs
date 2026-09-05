using UnityEngine;

public class FovBurst : CameraEffect
{
    [SerializeField] private float _sustainedFov = 8f;
    [SerializeField] private float _burstImpulse = 60f;
    [SerializeField] private float _frequency = 2.2f;
    [SerializeField] private float _damping = 0.8f;
    [SerializeField] private float _referenceSpeed = 25f;
    [SerializeField] private GoblinCharacter _character;


    private float _value;
    private float _velocity;
    private bool _wasSliding;

    public override float FovOffset => _value;

    public override void Tick(float deltaTime)
    {
        Debug.Log($"tick - sliding: {_character.Stance == Stance.Slide}, wasSliding: {_wasSliding}, value: {_value}, velocity: {_velocity}");
        bool sliding = _character.Stance == Stance.Slide;

        // on slide entry
        if (sliding && !_wasSliding)
        {
            float scale = Mathf.Clamp01(_character.HorizontalVelocityMagnitude() / _referenceSpeed);
            _velocity += _burstImpulse * scale;
        }

        _wasSliding = sliding;

        float target = sliding ? _sustainedFov : 0f;

        float omega = 2f * Mathf.PI * _frequency;
        float force = (target - _value) * omega * omega - _velocity * 2f * _damping * omega;

        _velocity += force * deltaTime;
        _value += _velocity * deltaTime;
    }
}
