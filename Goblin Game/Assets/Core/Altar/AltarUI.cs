using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarUI : MonoBehaviour
{
    public static AltarUI Instance {get; private set;}


    [SerializeField] CanvasGroup display;
    [SerializeField] LevelObjectives levelObjectives;
    [SerializeField] AltarOptionUI altarOptionPrefab;
    [SerializeField] RectTransform holder;

    private List<AltarOptionUI> altarOptions = new();


    void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    void Start()
    {
        display.alpha = 0;
        display.blocksRaycasts = false;

        StartCoroutine(InitializeDelay());
    }

    IEnumerator InitializeDelay()
    {
        yield return new WaitUntil(() => levelObjectives.ObjectiveCollections.Count > 0);

        Initialize();
    }

    public void Initialize()
    {
        foreach(var option in altarOptions)
            Destroy(option.gameObject);
        altarOptions.Clear();

        for(int i=0; i<levelObjectives.ObjectiveCollections.Count; i++)
        {
            AltarOptionUI altarOption = Instantiate(altarOptionPrefab, holder);
            altarOption.Initialize(levelObjectives.ObjectiveCollections[i], i, OnSelect);
            altarOptions.Add(altarOption);
        }
    }

    private void OnSelect(int collectionIndex)
    {
        levelObjectives.SetCollectionIndexServerRpc(collectionIndex);
        Hide();
    }

    public void Show()
    {
        display.alpha = 1;
        display.blocksRaycasts = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Hide()
    {
        display.alpha = 0;
        display.blocksRaycasts = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
