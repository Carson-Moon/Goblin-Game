using System.Collections.Generic;
using UnityEngine;

public class ObjectiveCollection : MonoBehaviour
{
    private Objective[] objectives;
    public Objective[] Objectives => objectives;


    void Start()
    {
        List<Objective> childObjectives = new();
        foreach(Transform child in transform)
        {
            if(child.TryGetComponent(out Objective objective))
                childObjectives.Add(objective);
        }
        objectives = childObjectives.ToArray();
    }
}
