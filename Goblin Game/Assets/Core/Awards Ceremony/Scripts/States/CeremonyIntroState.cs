using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class CeremonyIntroState : CeremonyState
{
    public override void StartState()
    {
        base.StartState();

        StartCoroutine(IntroQuip());
    }

    IEnumerator IntroQuip()
    {
        IntroQuipClientRpc();

        yield return new WaitForSeconds(2f);

        EndState();
    }

    [ClientRpc]
    private void IntroQuipClientRpc()
    {
        Debug.Log("Man, this ceremony sure is crazy!");
    }
}
