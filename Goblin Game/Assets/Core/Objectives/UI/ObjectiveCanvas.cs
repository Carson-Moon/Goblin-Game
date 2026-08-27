using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveCanvas : MonoBehaviour
{
    public static ObjectiveCanvas Instance {get; private set;}


    [SerializeField] ObjectivePanelUI panelPrefab;
    [SerializeField] RectTransform panelHolder;
    private List<ObjectivePanelUI> titlePanels = new();
    private Dictionary<ObjectiveCondition, ObjectivePanelUI> conditionPanels = new();


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
    }

    private ObjectivePanelUI CreatePanel(string display)
    {
        ObjectivePanelUI panel = Instantiate(panelPrefab, panelHolder);
        panel.Initialize(display);
        return panel;
    }

    public void UpdateConditionPanel(ObjectiveCondition condition)
    {
        if(conditionPanels.ContainsKey(condition))
            conditionPanels[condition].Initialize(condition.GetPanelDisplay());
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
