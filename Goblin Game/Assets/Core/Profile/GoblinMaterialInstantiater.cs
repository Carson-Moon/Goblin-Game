using System.Collections.Generic;
using UnityEngine;

public class GoblinMaterialInstantiater : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer[] skinnedMeshRens;

    private List<Material> materials = new();
    public List<Material> Materials => materials;


    void Start()
    {
        Initialize();   
    }

    public void Initialize()
    {
        if(materials.Count > 0)
            return;

        foreach(var meshRen in skinnedMeshRens)
        {
            for(int i=0; i<meshRen.materials.Length; i++)
            {
                meshRen.materials[i] = Instantiate(meshRen.materials[i]);
                materials.Add(meshRen.materials[i]);
            }
        }
    }
}
