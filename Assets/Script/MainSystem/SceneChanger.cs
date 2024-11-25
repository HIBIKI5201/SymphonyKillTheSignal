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
    /// シーンをロードし、AsyncOperationを戻り値とする
    /// </summary>
    /// <param name="sceneKind">シーンの種類</param>
    /// <returns>AsyncOperationクラス</returns>
    public static IEnumerator ChangeScene(SceneKind sceneKind)
    {
        // 新しいシーンを Additive モードでロード
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneNames[sceneKind], LoadSceneMode.Additive);
        //ロードが終わるまで待機
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneNames[sceneKind]));
        // ロード完了後に現在のシーンをアンロード
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(_sceneNames[CurrentScene]);
        //アンロードが終わるまで待機
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