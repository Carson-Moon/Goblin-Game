using UnityEngine;

public abstract class Stat : MonoBehaviour
{
    [SerializeField] string displayName;
    public string DisplayName => displayName;

    [SerializeField] float value;
    public float Value => value;


    public abstract void IncrementStat(float value);
    public void PackageForNetworkSending()
    {
        
    }
}
