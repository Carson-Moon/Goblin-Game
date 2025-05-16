using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject m_LeftArm;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject m_RightArm;
    [SerializeField] private Vacuum_Goblin m_VacuumGoblin;
    
    [Header("Nameplate")]
    [SerializeField] TextMeshPro namePlate;
    [SerializeField] string playerName;

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
        m_LeftArm.SetActive(false);
        namePlate.gameObject.SetActive(false);
        m_RightArm.SetActive(false);
        m_VacuumGoblin.enabled = false;
    }

    // Things to do once we establish our setup. Probably our own setup stuff!
    void Start()
    {
        // Move to a spawn point.
        if(IsOwner)
        {
            rb.position = Spawnpoint_Manager.instance.GetNextSpawnPosition().position;

            // Setup our nameplate.
            UpdateNameplateRPC(PlayerInformation_Manager.instance.GetPlayerName());

            print(NetworkManager.Singleton.LocalClientId);

            LobbyManager.instance.AddClientToListRPC(this);
        }       
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Enable our camera and input if we are the owner.
        if (IsOwner)
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
            m_LeftArm.SetActive(true);   
            m_RightArm.SetActive(true);
            m_VacuumGoblin.enabled = true;     
        }
        else
        {
            m_GoblinBody.enabled = true;
            m_GoblinHead.enabled = true;
            gameObject.layer = 7;
            namePlate.gameObject.SetActive(true);
        }

        ForceUpdateNameplatesRPC();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void ForceUpdateNameplatesRPC()
    {
        UpdateNameplateRPC(PlayerInformation_Manager.instance.GetPlayerName());
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void UpdateNameplateRPC(string name)
    {
        namePlate.text = name;
        playerName = name;
    }

    public void SwitchScene()
    {
        if(IsHost)
        {
            var status = NetworkManager.SceneManager.LoadScene("Test", LoadSceneMode.Single);
            if(status != SceneEventProgressStatus.Started)
            {
                Debug.Log("Scene load failed!");
            }
        }
    }

#region Getters
    public string GetName()
    {
        return playerName;
    }
#endregion
}
