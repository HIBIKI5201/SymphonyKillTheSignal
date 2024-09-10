using System.Collections;
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
        WorldManager,
    }
    static readonly Dictionary<SceneKind, string> _sceneNames = new()
    {
        { SceneKind.Title , "TitleScene"},
        { SceneKind.Home , "HomeScene"},
        { SceneKind.Story , "StoryScene"},
        { SceneKind.WorldManager, "WorldManagerScene" },
    };
    static readonly Dictionary<string, SceneKind> _sceneKinds = new()
    {
        { "TitleScene", SceneKind.Title},
        { "HomeScene", SceneKind.Home },
        { "StoryScene" , SceneKind.Story},
    };
    public static string SceneDictionary(SceneKind sceneKind)
    {
        return _sceneNames[sceneKind];
    }
    public static SceneKind SceneDictionary(string sceneName)
    {
        return _sceneKinds[sceneName];
    }
    /// <summary>
    /// �V�[�������[�h���AAsyncOperation��߂�l�Ƃ���
    /// </summary>
    /// <param name="sceneKind">�V�[���̎��</param>
    /// <returns>AsyncOperation�N���X</returns>
    public static IEnumerator ChangeScene(SceneKind sceneKind)
    {
        // �V�����V�[���� Additive ���[�h�Ń��[�h
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneNames[sceneKind], LoadSceneMode.Additive);
        //���[�h���I���܂őҋ@
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneNames[sceneKind]));
        // ���[�h������Ɍ��݂̃V�[�����A�����[�h
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(_sceneNames[CurrentScene]);
        //�A�����[�h���I���܂őҋ@
        while (!asyncUnload.isDone)
        {
            yield return null;
        }
        CurrentScene = sceneKind;
    }
    public static IEnumerator UnloadScene(SceneKind sceneKind)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(_sceneNames[sceneKind]);
        while (!asyncUnload.isDone)
        {
            yield return null;
        }
    }
}