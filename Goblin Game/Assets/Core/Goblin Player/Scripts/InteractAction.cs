using UnityEngine;

public class InteractAction : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] Transform interactTransform;
    [SerializeField] float interactLength;
    [SerializeField] float interactRadius;
    [SerializeField] LayerMask interactMask;
    private Interactable interactableCandidate;
    private Collider[] interactCols;



    public void AttemptInteract()
    {
        if(interactableCandidate == null)
            return;

        interactableCandidate.Interact();
    }

    void Update()
    {
        CheckForInteractables();
    }

    private void CheckForInteractables()
    {
        interactCols = Physics.OverlapCapsule(interactTransform.position, interactTransform.position + (interactTransform.forward * interactLength), interactRadius, interactMask);

        if (interactCols.Length > 0)
        {
            interactableCandidate = interactCols[0].GetComponent<Interactable>();
        }
        else
        {
            interactableCandidate = null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(interactTransform.position, interactTransform.position + (interactTransform.forward * interactLength));
    }
}
