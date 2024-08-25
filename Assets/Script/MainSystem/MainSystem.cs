using System;
using System.Collections;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    StoryManager _storyManager;
    public UserDataManager _userDataManager;

    ScreenEffectUI _screenEffect;
    PauseUI _pauseUI;

    AudioSource _soundEffectSource;
    AudioSource _BGMSource;
    [SerializeField]
    SoundDataBase soundEffects;
    [SerializeField]
    SoundDataBase BGMs;

    void Start()
    {
        //UI ToolKitを取得
        _screenEffect = GetComponentInChildren<ScreenEffectUI>();
        StartCoroutine(GameStartEffect());
        _pauseUI = GetComponentInChildren<PauseUI>();
        //マネージャークラスを取得
        _storyManager = GetComponentInChildren<StoryManager>();
        _userDataManager = GetComponentInChildren<UserDataManager>();
        //AudioSourceを取得する
        _soundEffectSource = GetComponentInChildren<AudioSource>();
        _BGMSource = GetComponent<AudioSource>();
        //ポーズUIを非表示
        if (SceneChanger.CurrentScene == SceneChanger.SceneKind.Title)
        {
            _pauseUI.HidePause();
        }
        //セーブデータを確認
        SaveDataManager._mainSaveData = SaveDataManager.Load();
        //シーンのシステムを起動
        FindAnyObjectByType<SystemBase>().SystemAwake(this);
    }

    IEnumerator GameStartEffect()
    {
        _screenEffect.ButtonUnactiveElement(true);
        _screenEffect.ScreenFadeIn(2);
        yield return new WaitForSeconds(2);
        _screenEffect.ButtonUnactiveElement(false);
    }

    public void GameStart(bool Continue)
    {
        //続きからボタンかつセーブデータがある場合
        if (Continue && SaveDataManager._mainSaveData != null)
        {
            StartCoroutine(SceneChange(SceneChanger.SceneKind.Home));
        }
        else
        {
            SaveDataManager.Save(new SaveData(DateTime.Now, 0, 0));
            StartCoroutine(SceneChange(SceneChanger.SceneKind.Story));
        }
        _userDataManager.saveData = SaveDataManager._mainSaveData;
    }

    public void DataSave()
    {
        SaveDataManager.Save(_userDataManager.saveData);
    }

    public void BackToHome()
    {
        StartCoroutine(SceneChange(SceneChanger.SceneKind.Home));
    }

    public void BackToTitle()
    {
        StartCoroutine(SceneChange(SceneChanger.SceneKind.Title));
    }

    public void StoryAction(StoryManager.StoryKind storyKind)
    {
        StartCoroutine(SceneChange(SceneChanger.SceneKind.Story, storyKind));
    }

    public void SoundPlay(int number, int soundNumber)
    {
        switch (number)
        {
            case 0:
                if (soundEffects.dataList.Count > soundNumber)
                {
                    _soundEffectSource.PlayOneShot(soundEffects.dataList[soundNumber]);
                }
                break;

            case 1:
                if (soundEffects.dataList.Count > soundNumber)
                {
                    _BGMSource.Stop();
                    _BGMSource.clip = BGMs.dataList[soundNumber];
                    _BGMSource.Play();
                }
                break;

            default:
                Debug.LogWarning("SoundPlayメソッドの範囲外です");
                break;
        }
    }
    IEnumerator SceneChange(SceneChanger.SceneKind sceneKind)
    {
        StartCoroutine(SceneChange(sceneKind, StoryManager.StoryKind.Story));
        yield break;
    }
    IEnumerator SceneChange(SceneChanger.SceneKind sceneKind, StoryManager.StoryKind storyKind)
    {
        //ボタンロックを起動
        _screenEffect.ButtonUnactiveElement(true);
        //フェードアウト演出
        _screenEffect.ScreenFadeOut(1);
        yield return new WaitForSeconds(1);
        //シーンをロードしてロード終了まで待つ
        yield return SceneChanger.ChangeScene(sceneKind);
        //システム系を初期化
        FindAnyObjectByType<SystemBase>()?.SystemAwake(this);
        //ロードしたシーンに応じて動きを変える
        switch (sceneKind)
        {
            case SceneChanger.SceneKind.Story:
                _pauseUI.RevealPause();
                _storyManager.SetStoryData(storyKind);
                break;
            case SceneChanger.SceneKind.Home:
                _pauseUI.RevealPause();
                break;
            case SceneChanger.SceneKind.Title:
                _pauseUI.HidePause();
                break;
        }
        yield return new WaitForSeconds(0.2f);
        //フェードイン演出
        _screenEffect.ScreenFadeIn(1.5f);
        yield return new WaitForSeconds(1.5f);
        //ボタンロックを解除
        _screenEffect.ButtonUnactiveElement(false);
    }
}
