using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ObjectiveCanvas : MonoBehaviour
{
    public static ObjectiveCanvas Instance {get; private set;}


    [SerializeField] ObjectivePanelUI panelPrefab;
    [SerializeField] RectTransform panelHolder;
    private List<ObjectivePanelUI> titlePanels = new();
    private Dictionary<ObjectiveCondition, ObjectivePanelUI> conditionPanels = new();


    private ObjectiveSignUI localSignUI = null;


    void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void Initialize(Objective objective)
    {
        DestroyUI();

        titlePanels.Add(CreatePanel(objective.ObjectiveName));
        foreach(var condition in objective.Conditions)
            conditionPanels.Add(condition, CreatePanel(condition.GetPanelDisplay()));

        // This is really dumb but whateva bro.
        if(localSignUI == null)
        {
            var signUIs = FindObjectsByType<ObjectiveSignUI>(findObjectsInactive: FindObjectsInactive.Exclude, sortMode: FindObjectsSortMode.None);
            foreach(var sign in signUIs)
            {
                NetworkObject netObject = sign.GetComponentInParent<NetworkObject>();
                if(netObject.OwnerClientId == NetworkManager.Singleton.LocalClientId)
                {
                    localSignUI = sign;
                    break;
                }
            }
        }

        if(localSignUI == null)
        {
            Debug.LogWarning("We did not find a valid sign...");
            return;
        }

        localSignUI.Show(objective);
    }

    public void ResetUI()
    {
        DestroyUI();

        if(localSignUI != null)
            localSignUI.Hide();
        else
            Debug.LogWarning("No sign found!");
    }

    public void UpdateConditionPanel(ObjectiveCondition condition)
    {
        if(conditionPanels.ContainsKey(condition))
            conditionPanels[condition].Initialize(condition.GetPanelDisplay());

        if(localSignUI != null)
            localSignUI.UpdateBottomText(condition);
        else
            Debug.LogWarning("No sign found!");
    }



    private ObjectivePanelUI CreatePanel(string display)
    {
        ObjectivePanelUI panel = Instantiate(panelPrefab, panelHolder);
        panel.Initialize(display);
        return panel;
    }

    private void DestroyUI()
    {
        foreach(var panel in titlePanels)
            Destroy(panel.gameObject);
        titlePanels.Clear();

        foreach(var panel in conditionPanels)
            Destroy(panel.Value.gameObject);
        conditionPanels.Clear();
    }
}
