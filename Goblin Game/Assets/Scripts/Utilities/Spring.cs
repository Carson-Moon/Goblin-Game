/*

    A simple spring implementation for simulating squash and stretch effects.

    Holds one float of spring state. When using, declare one per effect; where each will get its own inspector foldout with its own tuning.
    For use anywhere where something could react with physical weight.

    General Use:
    [SerializeField] private Spring mySpring = new Spring(180f, 12f);
    mySpring.Nudge(6f); // apply an impulse to the spring
    mySpring.target = 0f; // (or set it towards a moving value)

    void LateUpdate() {
        if (mySpring.IsAtRest) return;
        mySpring.Update(Time.deltaTime);
        transform.localScale = baseScale * (1f + mySpring.Value)
    }

    Caching:
    Cache the authored value. Read the original scale/position of the object in awake, and treat the spring value as a multiplier to that base value. 
    This allows you to change the base value in the editor without having to reset the spring.

    Tuning:
    Stiffness: How quickly the spring will return to its target value. Higher values = faster return.
    Damping: How quickly the spring will settle. Higher values = less oscillation.
    Max Amplitude: The maximum amount the spring can stretch or compress.
    Rest Threshold: The threshold at which the spring is considered to be at rest.

*/

using UnityEngine;

[System.Serializable]
public struct Spring {
    [SerializeField] private float stiffness;
    [SerializeField] private float damping;
    [SerializeField] private float maxAmplitude;
    [SerializeField] private float restThreshold;

    public float Value;
    public float Velocity;
    public float Target;

    public bool IsAtRest { get; private set; }

    public Spring(float stiffness, float damping, float maxAmplitude = 0.6f, float restThreshold = 0.001f) {
        this.stiffness = stiffness;
        this.damping = damping;
        this.maxAmplitude = maxAmplitude;
        this.restThreshold = restThreshold;
        Value = Velocity = Target = 0f;
        IsAtRest = true;
    }

    public void Nudge(float force) {
        Velocity += force;
        IsAtRest = false;
    }

    public void Update(float dt) {
        if (IsAtRest) return;

        float accel = (Target - Value) * stiffness - Velocity * damping;
        Velocity += accel * dt;
        Value    += Velocity * dt;

        // Clamp prevents a double-nudge from flattening or inverting the mesh
        if (Mathf.Abs(Value - Target) > maxAmplitude) {
            Value = Target + Mathf.Sign(Value - Target) * maxAmplitude;
            Velocity *= -0.3f;
        }

        // Settle
        if (Mathf.Abs(Value - Target) < restThreshold && Mathf.Abs(Velocity) < restThreshold) {
            Value = Target;
            Velocity = 0f;
            IsAtRest = true;
        }
    }

    public void Reset(float value = 0f) {
        Value = value;
        Velocity = 0f;
        IsAtRest = true;
    }
}