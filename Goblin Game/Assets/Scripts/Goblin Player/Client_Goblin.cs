using Unity.Netcode;
using UnityEngine;

// Responsible for allowing/disallowing behaviours based on client instance.

public class Client_Goblin : NetworkBehaviour
{
    // Behaviours to disable.
    [SerializeField] private Input_Goblin m_GoblinInput;
    [SerializeField] private Movement_Goblin m_MovementGoblin;
    [SerializeField] private Stab_Goblin m_StabGoblin;
    [SerializeField] private MouseLook_Goblin m_MouseLookGoblin;
    [SerializeField] private Camera m_Camera;
    [SerializeField] private AudioListener m_AudioListener;
    [SerializeField] private MeshRenderer m_GoblinBody;
    [SerializeField] private MeshRenderer m_GoblinHead;
    [SerializeField] private JarManager_Goblin m_JarManagerGoblin;
    [SerializeField] private CoinManager_Goblin m_CoinManagerGoblin;
    [SerializeField] private GameObject m_CanvasGoblin;

    void Awake()
    {
        m_GoblinInput.enabled = false;
        m_MovementGoblin.enabled = false;
        m_StabGoblin.enabled = false;
        m_MouseLookGoblin.enabled = false;
        m_Camera.enabled = false;
        m_AudioListener.enabled = false;
        m_GoblinBody.enabled = false;
        m_GoblinHead.enabled = false;
        m_JarManagerGoblin.enabled = false;
        m_CoinManagerGoblin.enabled = false;
        m_CanvasGoblin.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Enable our camera and input if we are the owner.
        if(IsOwner)
        {
            m_GoblinInput.enabled = true;
            m_MovementGoblin.enabled = true;
            m_StabGoblin.enabled = true;
            m_MouseLookGoblin.enabled = true;
            m_Camera.enabled = true;
            m_AudioListener.enabled = true;
            m_JarManagerGoblin.enabled = true;
            m_CoinManagerGoblin.enabled = true;
            m_CanvasGoblin.SetActive(true);
            
        }
        else
        {
            m_GoblinBody.enabled = true;
            m_GoblinHead.enabled = true;
            gameObject.layer = 7;
        }
    }
}
