using System.Collections;
using System.Collections.Generic;
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

    static Dictionary<SceneKind, string> _sceneNames = new Dictionary<SceneKind, string>()
    {
        { SceneKind.Title , "none"},
        { SceneKind.Home , "none"},
        { SceneKind.Story , "StoryScene"},
    };

    public static void ChangeScene(SceneKind sceneKind)
    {
        SceneManager.LoadScene(_sceneNames[sceneKind]);
    }
}
