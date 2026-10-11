using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class AltarOptionUI : MonoBehaviour
{
    [SerializeField] Animated_Button button;
    [SerializeField] TextMeshProUGUI titleText;

    private Action<int> onSelect;

    private ObjectiveCollection collection;

    private int collectionIndex;


    public void Initialize(ObjectiveCollection collection, int collectionIndex, Action<int> onSelect)
    {
        button.onButtonPressedAction += Select;

        this.onSelect += onSelect;

        this.collection = collection;
        titleText.text = collection.Objectives.First().ObjectiveName;

        this.collectionIndex = collectionIndex;
    }

    void OnDestroy()
    {
        button.onButtonPressedAction -= Select;
        this.onSelect = null;
    }

    public void Select()
    {
        onSelect?.Invoke(collectionIndex);
    }
}
