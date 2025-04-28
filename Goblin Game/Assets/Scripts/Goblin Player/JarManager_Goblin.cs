using UnityEngine;

// Responsible for picking up and performing actions with our jar.

public class JarManager_Goblin : MonoBehaviour
{
    // Runtime
    [SerializeField] Camera m_Camera;
    [SerializeField] Transform jarPosition;
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
        if(detectedJar == null)
            return;

        // Request ownership of the jar.
        detectedJar.GetComponent<Jar>().RequestOwnership();

        // Turn off jar physics.
        detectedJar.GetComponent<Jar>().DisablePhysics();

        // Put jar in hand.
        detectedJar.GetComponent<Jar>().SetJarPosition(jarPosition);       
    }

    // Attempt to throw a jar.
    public void AttemptThrow()
    {

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
