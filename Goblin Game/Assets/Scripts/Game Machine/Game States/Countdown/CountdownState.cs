using System.Collections;
using Unity.Netcode;
using UnityEngine;

// THIS STATE IS WHERE WE MAKE SURE EVERYTHING IS ENABLED FOR GAMEPLAY,
// AND WE PERFORM THE COUNTDOWN TO START THE MATCH.

public class CountdownState : GameState
{
    [SerializeField] CountdownCanvas countdownCanvas;
    [SerializeField] Client_Goblin clientGoblin;


    public override void StartThisState()
    {
        base.StartThisState();

        // Make sure our overlay camera is on!
        clientGoblin = ConnectedPlayerManager.instance.GetClientGoblin(NetworkManager.Singleton.LocalClientId);
        clientGoblin.SetOverlayCamera(true);

        FadeUI.StartFade(0, 1);

        // Start our countdown.
        countdownCanvas.StartCountdown();
        countdownCanvas.onCountdownEnd = null;
        countdownCanvas.onCountdownEnd += SetupPlayer;
    }

    // Set up our player for gameplay!
    private void SetupPlayer()
    {
        // Make sure our input is currently disabled!
        print("SETUP OUR PLAYER!");
        clientGoblin.SetMovement(true);

        // We are done with this state.
        EndThisState();
    }
}
