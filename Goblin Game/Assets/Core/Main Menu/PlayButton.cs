using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] string sceneDestinationName;


    public void OnPress()
    {
        SceneChange.ChangeScene(sceneDestinationName);
    }
}
