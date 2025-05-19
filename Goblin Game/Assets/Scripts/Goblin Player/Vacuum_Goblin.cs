using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Vacuum_Goblin : NetworkBehaviour
{
    // Runtime
    [SerializeField] bool isVacuuming = false;
    private Collider[] m_VacuumedObjects;
    private Vector3 m_VacuumEndPos;
    private int length;
    private Vector3 m_VacuumDirection;
    private float m_VacuumDistance;
    private float m_DistancePercentage;
    private float m_CurrentStrength;
    private float m_CurrentUpwardsStrength;
    private Rigidbody m_VacuumedRigidbody;

    [Header("Vacuum Settings")]
    [SerializeField] float vacuumLength;
    [SerializeField] float vacuumWidth;
    [SerializeField] Transform vacuumStartPos;
    [SerializeField] float vacuumStrength;
    [SerializeField] float vacuumTorqueStrength;
    [SerializeField] float upwardsStrength;
    [SerializeField] LayerMask vacuumableMask;

    [Header("Animation Settings")]
    [SerializeField] private Animator m_Anim;
    private int m_StartVacuumHash = Animator.StringToHash("startVacuum");
    private int m_EndVacuumHash = Animator.StringToHash("endVacuum");


    void Update()
    {
        if (isVacuuming)
        {
            // Perform vacuum check and force.
            PerformVacuum();
        }
    }

    // Perform vacuum.
    private void PerformVacuum()
    {
        // Setup our vacuum end.
        m_VacuumEndPos = (vacuumStartPos.forward * vacuumLength) + vacuumStartPos.position;

        // Perform a box check and grab any objects in range.
        m_VacuumedObjects = Physics.OverlapCapsule(vacuumStartPos.position, m_VacuumEndPos, vacuumWidth, vacuumableMask);

        // If we have any vacuumed objects, add a force to them towards the vacuum.
        length = m_VacuumedObjects.Length;
        for (int i = 0; i < length; i++)
        {
            // Get direction and distance from this object to the vacuum.
            m_VacuumDirection = (vacuumStartPos.position - m_VacuumedObjects[i].transform.position).normalized;
            m_VacuumDistance = Vector3.Distance(vacuumStartPos.position, m_VacuumedObjects[i].transform.position);

            // Get distance percentage.
            m_DistancePercentage = 1f - (m_VacuumDistance / vacuumLength);

            // Strength is based on max strength subtracted by distance.
            m_CurrentStrength = vacuumStrength * m_DistancePercentage;

            // Get the objects rigidbody.
            m_VacuumedRigidbody = m_VacuumedObjects[i].GetComponent<Rigidbody>();

            // If the object is below our vacuum position, apply upwards strength.
            m_CurrentUpwardsStrength = 0;
            if (m_VacuumedObjects[i].transform.position.y < vacuumStartPos.position.y)
            {
                m_CurrentUpwardsStrength = upwardsStrength;
            }

            // Add some lift so the coins go up?
            Vector3 force = m_CurrentStrength * Time.deltaTime * ((Vector3.up * m_CurrentUpwardsStrength) + m_VacuumDirection);
            Vector3 torque = vacuumTorqueStrength * Time.deltaTime * m_VacuumedRigidbody.transform.right;
            m_VacuumedObjects[i].GetComponent<Vacuumable>().ApplyForceToThisRPC(force, torque);
        }
    }

    // Start vacuuming.
    public void OnVacuum()
    {
        isVacuuming = true;

        m_Anim.SetTrigger(m_StartVacuumHash);
    }

    // Stop vacuuming.
    public void OffVacuum()
    {
        isVacuuming = false;

        m_Anim.SetTrigger(m_EndVacuumHash);
    }
}
