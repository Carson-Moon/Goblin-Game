using System.Collections.Generic;
using UnityEngine;

public class LevelObjectives : MonoBehaviour
{
    private ObjectiveCollection[] objectiveCollections;


    void Start()
    {
        List<ObjectiveCollection> collections = new();
        foreach(Transform child in transform)
        {
            if(child.TryGetComponent(out ObjectiveCollection collection))
                collections.Add(collection);
        }
        objectiveCollections = collections.ToArray();
    }

    public Vector2Int GetRandomObjectiveIndex()
    {
        if(objectiveCollections.Length == 0)
            return new Vector2Int(-1, -1);

        int collectionIndex = Random.Range(0, objectiveCollections.Length);
        int objectiveIndex = Random.Range(0, objectiveCollections[collectionIndex].Objectives.Length);

        return new Vector2Int(collectionIndex, objectiveIndex);
    }

    public Objective GetObjectiveByIndex(Vector2Int objectiveIndex)
    {
        if(objectiveIndex.x < 0 || objectiveIndex.y < 0)
        {
            Debug.Log("Objective index is not valid.");
            return null;
        }
        else
            return objectiveCollections[objectiveIndex.x].Objectives[objectiveIndex.y];
    }
}
