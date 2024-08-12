using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public enum SceneKind 
    {
        Title,
        Home,
        Story,
    }

    static readonly Dictionary<SceneKind, string> _sceneNames = new()
    {
        { SceneKind.Title , "none"},
        { SceneKind.Home , "none"},
        { SceneKind.Story , "StoryScene"},
    };

    public static AsyncOperation ChangeScene(SceneKind sceneKind)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneNames[sceneKind]);
        return asyncLoad;
    }
}
