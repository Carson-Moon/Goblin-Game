using UnityEngine;

public class PickupVisual : MonoBehaviour
{
    [SerializeField] private PickupID _id;
    public PickupID ID => _id;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ToggleVisual(bool toggle)
    {
        gameObject.SetActive(toggle);
    }
}
