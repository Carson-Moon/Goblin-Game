using UnityEngine;

public class CameraEffectStack : MonoBehaviour
{
    private CameraEffect[] _effects;

    public Vector3 PositionOffset { get; private set; }
    public Vector3 EulerOffset { get; private set; }
    public float FovOffset { get; private set; }

    public void Initialize()
    {
        _effects = GetComponents<CameraEffect>();
    }

    public void Tick(float deltaTime)
    {
        var position = Vector3.zero;
        var euler = Vector3.zero;
        var fov = 0f;

        foreach (var effect in _effects)
        {
            if (!effect.enabled) continue;

            effect.Tick(deltaTime);
            position += effect.PositionOffset;
            euler += effect.EulerOffset;
            fov += effect.FovOffset;
        }

        PositionOffset = position;
        EulerOffset = euler;
        FovOffset = fov;
    }
}
