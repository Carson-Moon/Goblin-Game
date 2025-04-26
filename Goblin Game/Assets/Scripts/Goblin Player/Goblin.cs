using UnityEngine;

public class Goblin : MonoBehaviour
{
    public static Goblin instance {get; private set;}

    [Header("Goblin Logic")]
    [SerializeField] Transform orientation;


    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

#region Getters
    public Transform GetOrientation()
    {
        return orientation;
    }
#endregion
}
