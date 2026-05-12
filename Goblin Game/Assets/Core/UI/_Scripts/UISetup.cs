using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISetup : MonoBehaviour
{
    private IEnumerable<UIComponent> uiComponents;


    void Awake()
    {
        uiComponents = FindObjectsByType<UIComponent>(FindObjectsSortMode.None);
    }

    void Start()
    {
        foreach(var ui in uiComponents)
            ui.Initialize();
    }
}
