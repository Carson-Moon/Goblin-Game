using UnityEngine;

public class VacuumAction : MonoBehaviour
{
    [SerializeField] bool isVacuuming = false;
    public bool IsVacuuming => isVacuuming;

    [Header("Visuals")]
    [SerializeField] VacuumVFX vacuumVFX;
    [SerializeField] Animator anim;
    private int StartVacuumHash = Animator.StringToHash("startVacuum");
    private int EndVacuumHash = Animator.StringToHash("endVacuum");

    public void AttemptStartVacuum()
    {
        isVacuuming = true;

        anim.SetTrigger(StartVacuumHash);
        vacuumVFX.EnableTornado();
        Debug.Log("Started vacuuming!");
    }

    public void AttemptStopVacuum()
    {
        isVacuuming = false;

        anim.SetTrigger(EndVacuumHash);
        vacuumVFX.DisableTornado();
        Debug.Log("Stopped vacuuming.");
    }
}
