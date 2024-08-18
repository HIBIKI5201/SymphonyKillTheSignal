using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSystemLoader : MonoBehaviour
{
    [SerializeField]
    string MainSystemSceneName = "SystemScene";

    private void Start()
    {
        if (!SceneManager.GetSceneByName(MainSystemSceneName).isLoaded)
        {
            SceneManager.LoadScene(MainSystemSceneName, LoadSceneMode.Additive);
            Scene currentScene = SceneManager.GetActiveScene();
            SceneChanger.CurrentScene = SceneChanger.SceneDictionary(currentScene.name);
        }
        Destroy(gameObject);
    }
}
