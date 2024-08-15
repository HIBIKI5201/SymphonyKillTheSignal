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
    /// <summary>
    /// �V�[�������[�h���AAsyncOperation��߂�l�Ƃ���
    /// </summary>
    /// <param name="sceneKind">�V�[���̎��</param>
    /// <returns>AsyncOperation�N���X</returns>
    public static AsyncOperation ChangeScene(SceneKind sceneKind)
    {
        return SceneManager.LoadSceneAsync(_sceneNames[sceneKind]);
    }
}
