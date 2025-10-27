using UnityEngine;

public class GoblinAnimator : MonoBehaviour
{
    // Runtime
    private GoblinCharacter gCharacter;

    [SerializeField] Animator anim;

    [Space(10)]

    [Header("Movement Animation Settings")]
    [SerializeField] float maxRunningThreshold;
    [SerializeField] float blendResponse;
    private int MoveHash = Animator.StringToHash("movementBlend");
    private float currentMovementBlend;
    private float targetMovementBlend;


    void Awake()
    {
        gCharacter = GetComponent<GoblinCharacter>();
    }

    void Update()
    {
        targetMovementBlend = gCharacter.HorizontalVelocityMagnitude() / maxRunningThreshold;
        targetMovementBlend = Mathf.Clamp(targetMovementBlend, 0, 1);

        currentMovementBlend = Mathf.Lerp
        (
            a: currentMovementBlend,
            b: targetMovementBlend,
            t: 1f - Mathf.Exp(-blendResponse * Time.deltaTime)
        );
  
        anim.SetFloat(MoveHash, currentMovementBlend);
    }
}
