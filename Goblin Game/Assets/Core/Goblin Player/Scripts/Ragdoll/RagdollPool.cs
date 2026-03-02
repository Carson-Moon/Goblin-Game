using System.Collections.Generic;
using UnityEngine;

public class RagdollPool : MonoBehaviour
{
    public static RagdollPool Instance {get; private set;}

    [Header("Pool Settings")]
    [SerializeField] int numRagdollsInPool = 10;
    [SerializeField] GoblinRagdoll goblinRagdollPrefab;
    [SerializeField] List<GoblinRagdoll> ragdolls = new();


    void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        Setup();
    }

    public void Setup()
    {
        for(int i=0; i<numRagdollsInPool; i++)
            ragdolls.Add(Instantiate(goblinRagdollPrefab, transform));
    }

    public GoblinRagdoll GetRagdoll(Transform ragdollPosition, Vector3 impactPoint)
    {
        if(ragdolls.Count > 0)
        {
            var ragdoll = ragdolls[0];
            ragdolls.Remove(ragdoll);
            ragdoll.Ragdoll(ragdollPosition.position, ragdollPosition.rotation, impactPoint);

            return ragdoll;
        }
        else
        {
            Debug.LogError("We have run out of ragdolls! Logic for spawning a new one here.");
            return null;
        }
    }

    public void ReturnRagdoll(GoblinRagdoll goblinRagdoll)
    {
        goblinRagdoll.ResetRagdoll();

        ragdolls.Add(goblinRagdoll);
    }
}
