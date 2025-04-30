using UnityEngine;

// Responsible for picking up and performing actions with our jar.

public class JarManager_Goblin : MonoBehaviour
{
    // Runtime
    [SerializeField] Camera m_Camera;
    [SerializeField] Transform jarPosition;
    [SerializeField] private bool m_HasJar = false;
    [SerializeField] private Jar m_CurrentJar = null;
    private RaycastHit m_Hit;
    [SerializeField] private float m_JarThrowStrength;

    [Header("Jar Detection Settings")]
    [SerializeField] LayerMask jarLayer;
    [SerializeField] float rayLength;
    [SerializeField] GameObject detectedJar;

    [Header("Pickup Timer")]
    [SerializeField] private bool m_PickingUp;
    [SerializeField] private float pickupCooldownLength;
    private float m_PickupCooldown;

    [Header("UI")]
    [SerializeField] GameObject jarIndicator;


    void Awake()
    {
        m_PickupCooldown = pickupCooldownLength;
    }

    void Update()
    {
        // Perform our jar raycast.
        DetectJar(); 

        // If we are picking up, apply cooldown.
        if(m_PickingUp)
        {
            m_PickupCooldown -= Time.deltaTime;
            m_PickupCooldown = Mathf.Clamp(m_PickupCooldown, 0, pickupCooldownLength);

            if(m_PickupCooldown <= 0)
            {
                AttemptPickup();

                StopAttemptPickup();
            }
        }
    }

    // Grabs a reference to a jar if we are detecting one.
    private void DetectJar()
    {
        if(Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out m_Hit, rayLength, jarLayer))
        {
            detectedJar = m_Hit.transform.gameObject;

            jarIndicator.SetActive(true);
        }
        else
        {
            detectedJar = null;

            jarIndicator.SetActive(false);
        }
    }

    // Start attempting to pickup.
    public void StartAttemptPickup()
    {
        if(detectedJar == null || m_HasJar)
            return;

        m_PickingUp = true;

        detectedJar.GetComponent<Jar>().RequestOwnership();
    }

    // Stop attempting to pickup.
    public void StopAttemptPickup()
    {
        m_PickingUp = false;

        m_PickupCooldown = pickupCooldownLength;       
    }

    // Attempt to pick up a detectedJar.
    public void AttemptPickup()
    {
        if(detectedJar == null || m_HasJar)
            return;

        m_CurrentJar = detectedJar.GetComponent<Jar>();

        // Request ownership.
        m_CurrentJar.RequestOwnership();

        m_HasJar = true;

        // Turn off jar physics.
        m_CurrentJar.DisablePhysics();

        // Put jar in hand.
        m_CurrentJar.SetJarPosition(jarPosition);       
    }

    // Attempt to throw a jar.
    public void AttemptThrow()
    {
        if(m_CurrentJar == null)
            return;

        // Turn on jar physics.
        m_CurrentJar.EnablePhysics();

        // Apply throw force to jar.
        m_CurrentJar.ImpulseInDirection(jarPosition.forward, m_JarThrowStrength);

        // Un-set jar position.
        m_CurrentJar.SetJarPosition(null);

        // Forget anything about a jar. Whats a jar?
        m_CurrentJar = null;
        m_HasJar = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8)
        {
            print("I just hit a jar!");
            collision.gameObject.GetComponent<Jar>().RequestOwnership();
        }
    }
}
