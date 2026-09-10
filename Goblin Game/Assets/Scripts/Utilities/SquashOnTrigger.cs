using UnityEngine;

public class SquashOnTrigger : MonoBehaviour
{
    [SerializeField] private Spring squashSpring = new Spring(180f, 12f);
    [SerializeField] private float ringImpulse = 6f;
    [SerializeField] private Transform meshRoot;

    [SerializeField] private LayerMask triggeringLayers;
    private Vector3 baseScale;
    

    void Awake()
    {
        baseScale = meshRoot.localScale;
    }

    void LateUpdate()
    {
        if (squashSpring.IsAtRest) return;
        squashSpring.Update(Time.deltaTime);
        float s = squashSpring.Value;
        meshRoot.localScale = Vector3.Scale(baseScale, new Vector3(1f + s, 1f - s, 1f + s));
    }

    void OnTriggerEnter(Collider other)
    {
        if ((triggeringLayers.value & (1 << other.gameObject.layer)) == 0) return;
        squashSpring.Nudge(ringImpulse);
    }

    
}
