using UnityEngine;

public class DipTest : MonoBehaviour
{
    [SerializeField] GoblinCharacter character;
    [SerializeField] float frequency = 4f, damping = 0.5f, scale = 0.004f;
    public float dip, dipVel;

    void OnEnable() => character.Landed += OnLanded;
    void OnDisable() => character.Landed -= OnLanded;
    void OnLanded(float speed) => dipVel -= speed * scale;

    [SerializeField] float pitchRatio = 0f;
    public Vector3 PositionOffset => transform.up * dip;
    public Vector3 EulerOffset => new Vector3(-dip * pitchRatio, 0f, 0f);

    // Update is called once per frame
    void Update()
    {
        float omega = 2f * Mathf.PI * frequency;
        float dt = Mathf.Min(Time.deltaTime, 1f / 30f);
        dipVel += (-omega * omega * dip - 2f * damping * omega * dipVel) * dt;
        dip += dipVel * dt;
    }
}
