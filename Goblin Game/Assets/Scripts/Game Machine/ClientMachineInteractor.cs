using UnityEngine;

public class ClientMachineInteractor : MonoBehaviour
{
    // Runtime
    [SerializeField] ClientGameMachine clientMachine;


    void Start()
    {
        clientMachine = GameObject.FindGameObjectWithTag("Client Machine").GetComponent<ClientGameMachine>();
    }

    // Check if we are ready to switch states.
    public bool CheckStateSwitchReadiness()
    {
        return clientMachine.readyToSwitch;
    }
}
