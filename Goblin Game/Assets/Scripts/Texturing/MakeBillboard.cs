using UnityEngine;

public class MakeBillboard : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main == null)
            return;
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0f, 180f, 0f);
        
    }
}
