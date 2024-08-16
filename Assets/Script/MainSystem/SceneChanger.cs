using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    static public SceneKind CurrentScene = SceneKind.Title;
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
    /// <summary>
    /// �V�[�������[�h���AAsyncOperation��߂�l�Ƃ���
    /// </summary>
    /// <param name="sceneKind">�V�[���̎��</param>
    /// <returns>AsyncOperation�N���X</returns>
    public static AsyncOperation ChangeScene(SceneKind sceneKind)
    {
        CurrentScene = sceneKind;
        return SceneManager.LoadSceneAsync(_sceneNames[sceneKind]);
    }
}
