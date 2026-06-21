using UnityEngine;

public class TrajectoryVisualizer : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform releasePosition;
    [SerializeField] Camera cam;

    [Header("Display")]
    [SerializeField] private int linePoints = 25;
    [SerializeField] private float timeBetweenPoints = 0.1f;
    [SerializeField] private float yOffset = 2f;

    private bool visualizing = false;
    private float throwStrength;
    private float projectileMass;


    void Update()
    {
        if(visualizing)
            DrawProjection();
    }

    public void StartVisualizing(float throwStrength, float projectileMass)
    {
        lineRenderer.enabled = true;
        this.throwStrength = throwStrength;
        this.projectileMass = projectileMass;
        visualizing = true;
    }

    public void StopVisualizing()
    {
        lineRenderer.enabled = false;
        visualizing = false;
    }

    public void DrawProjection()
    {
        lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
        Vector3 startPosition = releasePosition.position - new Vector3(0, yOffset, 0);
        Vector3 startVelocity =  throwStrength * cam.transform.forward / projectileMass;
        int i = 0;
        lineRenderer.SetPosition(i, startPosition);
        for(float time = 0; time< linePoints; time += timeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            lineRenderer.SetPosition(i, point);
        }
    }
}
