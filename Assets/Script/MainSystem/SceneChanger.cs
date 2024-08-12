using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public enum SceneKind 
    {
        Title,
        Home,
        Story,
    }

    Dictionary<SceneKind, string> _sceneNames = new Dictionary<SceneKind, string>()
    {
        { SceneKind.Title , "none"},
        { SceneKind.Home , "none"},
        { SceneKind.Story , "StoryScene"},
    };
}
