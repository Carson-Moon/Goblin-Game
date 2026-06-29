using Unity.Netcode;
using Unity;

public abstract class CeremonyState : NetworkBehaviour
{
    private bool running = false;
    public bool Running => running;

    public CeremonyState nextState;


    public virtual void StartState()
    {
        running = true;
    }
    
    public virtual void UpdateState()
    {
        
    }

    public virtual void EndState()
    {
        running = false;
    }
}
