using UnityEngine;
using UnityEngine.VFX;

public class SpawnObjectAtPoint : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public GameObject objectToSpawn; 
    public GameObject spawnVFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (spawnPoint == null)
        {
            Debug.LogError("Spawn point is not assigned.");
        }
    }
    private void SpawnObject()
    {
        if (objectToSpawn == null)
        {
            Debug.LogError("Object to spawn is not assigned.");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("Spawn point is not assigned.");
            return;
        }

        if (spawnVFX != null)
        {
            GameObject vfxInstance = Instantiate(spawnVFX, spawnPoint.position, spawnPoint.rotation);
            Destroy(vfxInstance, 2f); // Destroy the VFX after 2 seconds
        }

        Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnObject();
        }
    }
}
