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
        { SceneKind.Title , "TitleScene"},
        { SceneKind.Home , "HomeScene"},
        { SceneKind.Story , "StoryScene"},
    };

    public static AsyncOperation ChangeScene(SceneKind sceneKind)
    {
        return SceneManager.LoadSceneAsync(_sceneNames[sceneKind]);
    }
}
