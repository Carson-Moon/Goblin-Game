using UnityEngine;

public class LandingDipEffect : CameraEffect
{
    [Header("Spring Settings")]
    [SerializeField] float frequency = 4f;
    [Range(0f, 1f), SerializeField] float dampingRatio = 0.5f;

    [Header("Impact Settings")]
    [SerializeField] float dipPerSpeed = 0.004f;
    [SerializeField] float maxImpactSpeed = 60f;
    [SerializeField] AnimationCurve response = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [Header("Pitch")]
    [SerializeField] float pitchRatio = 0f;

    private GoblinCharacter _character;
    private float _dip;
    private float _dipVelocity;

    public override Vector3 PositionOffset => new Vector3(0f, _dip, 0f);
    public override Vector3 EulerOffset => new Vector3(-_dip * pitchRatio, 0f, 0f);

    public void Initialize(GoblinCharacter character)
    {
        _character = character;
        _character.Landed += OnLanded;
    }

    public void OnDestroy()
    {
        if (_character != null)
            _character.Landed -= OnLanded;
    }

    private void OnLanded(float impactSpeed)
    {
        var t = Mathf.Clamp01(impactSpeed / maxImpactSpeed);
        _dipVelocity -= response.Evaluate(t) * maxImpactSpeed * dipPerSpeed;
        // Debug.Log($"LandingDipEffect: OnLanded called with impactSpeed={impactSpeed}, t={t}, dipVelocity={_dipVelocity}");
    }

    public override void Tick(float deltaTime)
    {
        var omega = 2f * Mathf.PI * frequency;
        var dt = Mathf.Min(deltaTime, 1f / 30f);

        _dipVelocity += (-omega * omega * _dip - 2f * dampingRatio * omega * _dipVelocity) * dt;
        _dip += _dipVelocity * dt;

        if (Mathf.Abs(_dip) < 1e-5f && Mathf.Abs(_dipVelocity) < 1e-5f)
        {
            _dip = 0f;
            _dipVelocity = 0f;
        }
    }


}
