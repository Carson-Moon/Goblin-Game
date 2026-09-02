using UnityEngine;

public class CoinAnimator : MonoBehaviour
{
    private float startingY;
    [SerializeField] float frequency;
    [SerializeField] float amplitude;

    [SerializeField] float rotateSpeed;


    void Awake()
    {
        startingY = transform.position.y;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x, startingY + yOffset, transform.position.z);

        transform.Rotate(transform.up * rotateSpeed * Time.deltaTime);
    }
}
