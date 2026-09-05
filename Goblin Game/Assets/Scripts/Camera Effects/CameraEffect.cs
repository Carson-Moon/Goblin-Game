using UnityEngine;

/// <summary>
/// Base class for camera effects. Inherit from this class to create custom camera effects.
/// </summary>

public abstract class CameraEffect : MonoBehaviour
{
    // what does the arrow operator do in C#?
    // 
    public virtual Vector3 PositionOffset => Vector3.zero;
    public virtual Vector3 EulerOffset => Vector3.zero;
    public virtual float FovOffset => 0f;

    public abstract void Tick(float deltaTime);
}
