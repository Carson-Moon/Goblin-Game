using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

// Responsible for the initial setup of the ceremony scene.

public class CeremonySetupState : CeremonyState
{
    [SerializeField] IntStat[] intStats;
    [SerializeField] FloatStat[] floatStats;

    public IEnumerable<string> AllStatStrings => intStats.Select(x => x.ToString()).Concat(floatStats.Select(x => x.ToString()));

    [SerializeField] CeremonyWheel wheelUI;
    [SerializeField] SpawnPointSetter spawnPointSetter;


    public override void StartState()
    {
        base.StartState();

        spawnPointSetter.MovePlayersToSpawnPoints();

        List<string> categoryNames = new();
        for(int i=0; i<intStats.Length; i++)
            categoryNames.Add(intStats[i].ToString());
        for(int i=0; i<floatStats.Length; i++)
            categoryNames.Add(floatStats[i].ToString());

        var categoriesNetworkedArray = new NetworkedStringArray
        {
            StringArray = categoryNames.ToArray()
        };
        SetupStatWheelClientRpc(categoriesNetworkedArray);

        EndState();
    }

    [ClientRpc]
    private void SetupStatWheelClientRpc(NetworkedStringArray categories)
    {
        wheelUI.SetupWheel(categories.StringArray);
    }
}
