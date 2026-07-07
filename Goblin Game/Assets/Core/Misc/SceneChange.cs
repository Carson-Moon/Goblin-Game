using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneChange
{
    public static void ChangeScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
