using System.Collections.Generic;
using UnityEngine;

public class ObjectiveList : MonoBehaviour
{
    [SerializeField] List<Objective> allObjectives = new();


    public Objective GetObjectiveByIndex(int objectiveIndex)
    {
        return allObjectives[objectiveIndex];
    }

    public int GetRandomObjectiveIndex()
    {
        return Random.Range(0, allObjectives.Count);
    }

    public int GetRandomObjectiveVariation(int objectiveIndex)
    {
        return Random.Range(0, allObjectives[objectiveIndex].Variations);
    }
}
