using UnityEngine;

public class Follow_Goblin : MonoBehaviour
{
    [SerializeField] private Rigidbody _body;
    [SerializeField] private Vector3 _offset;


    void Update()
    {
        // Set our position to our goblin body position.
        transform.position = _body.position + _offset;
    }

    public void SetOffset(Vector3 value)
    {
        _offset = value;
    }
}
