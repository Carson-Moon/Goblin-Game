using UnityEngine;

// Randomize rotation of object on placement into scene based on some axis.

public class RandomRotationOnPlacement : MonoBehaviour
{
    public bool hasBeenRandomized = false;
    public bool randomizeX = false;
    public bool randomizeY = false;
    public bool randomizeZ = false;

    private void Reset()
    {
        if (!hasBeenRandomized)
        {
            Vector3 newRotation = transform.rotation.eulerAngles;

            if (randomizeX)
                newRotation.x = Random.Range(0f, 360f);
            if (randomizeY)
                newRotation.y = Random.Range(0f, 360f);
            if (randomizeZ)
                newRotation.z = Random.Range(0f, 360f);

            transform.rotation = Quaternion.Euler(newRotation);
            hasBeenRandomized = true;
        }
    }
}
