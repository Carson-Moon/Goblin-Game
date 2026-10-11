using Unity.Netcode;
using UnityEngine;

public class AltarController : NetworkBehaviour
{
    [SerializeField] AltarInteractable[] altars;
    [SerializeField] LevelObjectives levelObjectives;

    [SerializeField] bool altarsAvailable = true;
    public bool AltarsAvailable => altarsAvailable;

    void Awake()
    {
        altars = FindObjectsByType<AltarInteractable>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach(var altar in altars)
            altar.Initialize(this, OnAltarInteracted);
    }

    private void OnAltarInteracted()
    {
        OnAltarInteractedServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnAltarInteractedServerRpc()
    {
        OnAltarInteractedClientRpc();
    }

    [ClientRpc]
    private void OnAltarInteractedClientRpc()
    {
        altarsAvailable = false;

        foreach(var altar in altars)
            altar.animator.SetTrigger("Deactivate");
    }


    [ClientRpc]
    public void ResetAltarsClientRpc()
    {
        altarsAvailable = true;

        foreach(var altar in altars)
            altar.animator.SetTrigger("Activate");
    }
}
