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

    [Header("Jar Detection Settings")]
    [SerializeField] LayerMask jarLayer;
    [SerializeField] float rayLength;
    [SerializeField] GameObject detectedJar;

    [Header("UI")]
    [SerializeField] GameObject jarIndicator;


    void Update()
    {
        // Perform our jar raycast.
        DetectJar(); 
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

    // Attempt to pick up a detectedJar.
    public void AttemptPickup()
    {
        if(detectedJar == null || m_HasJar)
            return;

        m_HasJar = true;
        m_CurrentJar = detectedJar.GetComponent<Jar>();

        // Request ownership of the jar.
        m_CurrentJar.RequestOwnership();

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
        m_CurrentJar.ImpulseInDirection(jarPosition.forward, 10);

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
