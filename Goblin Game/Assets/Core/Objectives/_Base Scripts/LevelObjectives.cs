using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LevelObjectives : NetworkBehaviour
{
    private List<ObjectiveCollection> objectiveCollections =  new();
    public List<ObjectiveCollection> ObjectiveCollections => objectiveCollections;

    public int chosenCollectionIndex;


    void Start()
    {
        List<ObjectiveCollection> collections = new();
        foreach(Transform child in transform)
        {
            if(child.TryGetComponent(out ObjectiveCollection collection) && child.gameObject.activeInHierarchy)
                collections.Add(collection);
        }
        objectiveCollections = collections;
    }

    public Vector2Int GetRandomObjectiveIndex()
    {
        if(objectiveCollections.Count == 0)
            return new Vector2Int(-1, -1);

        int collectionIndex = chosenCollectionIndex != -1 ? chosenCollectionIndex : Random.Range(0, objectiveCollections.Count);
        int objectiveIndex = Random.Range(0, objectiveCollections[collectionIndex].Objectives.Length);

        chosenCollectionIndex = -1;

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

    [ServerRpc(RequireOwnership = false)]
    public void SetCollectionIndexServerRpc(int index)
    {
        chosenCollectionIndex = index;
    }
}
