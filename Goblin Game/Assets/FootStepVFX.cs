using UnityEngine;
using UnityEngine.VFX;

public class FootStepVFX : MonoBehaviour
{
    // This script handles footfalls and triggering smoke poof VFX when the foot hits the ground.

    [SerializeField] private VisualEffect leftFootStep;
    [SerializeField] private VisualEffect rightFootStep;

    public void RightFootHit() => Play(rightFootStep);
    public void LeftFootHit() => Play(leftFootStep);

    private void Play(VisualEffect vfx)
    {
        if (vfx == null) return;
        vfx.Play();
    }
}