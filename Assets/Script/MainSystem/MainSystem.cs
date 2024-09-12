using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    StoryManager _storyManager;
    public UserDataManager _userDataManager;

    ScreenEffectUI _screenEffect;
    PauseUI _pauseUI;

    public enum AudioPlayKind
    {
        SE,
        BGM,
    }

    AudioSource _soundEffectSource;
    AudioSource _BGMSource;
    AudioSource _voiceSource;
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
        AudioSource[] audioSources = transform.GetChild(0).GetComponents<AudioSource>();
        _soundEffectSource = audioSources[0];
        _BGMSource = audioSources[1];
        _voiceSource = audioSources[2];
        //ポーズUIを非表示
        if (SceneChanger.CurrentScene == SceneChanger.SceneKind.Title)
        {
            _pauseUI.HidePause();
        }
        //セーブデータを確認
        SaveDataManager._mainSaveData = SaveDataManager.Load();
        _userDataManager.saveData = SaveDataManager._mainSaveData;
        _storyManager.Initialized(this);
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
        if (Continue && SaveDataManager._mainSaveData != null && SaveDataManager._mainSaveData.time > 0)
        {
            StartCoroutine(SceneChange(SceneChanger.SceneKind.Home));
        }
        else
        {
            SaveDataManager.Save(new SaveData(0, 0, 100, 80, 100, 0, WorldManager.Weather.sunny, Enumerable.Repeat(false, _storyManager._storyData.Count).ToList()));
            _storyManager.Initialized(this);
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

    public void SoundPlay(AudioPlayKind kind, int soundNumber)
    {
        switch (kind)
        {
            case AudioPlayKind.SE:
                if (soundEffects.dataList.Count > soundNumber)
                {
                    DOTween.To(() => 1f, x => _soundEffectSource.volume = x, 0f, 3)
                        .OnStart(() =>
                        {
                            if (_soundEffectSource.clip != null) _soundEffectSource.Stop();
                            _soundEffectSource.clip = soundEffects.dataList[soundNumber];
                            _soundEffectSource.Play();
                        });
                }
                break;

            case AudioPlayKind.BGM:
                if (soundEffects.dataList.Count > soundNumber)
                {
                    DOTween.To(() => 1f, x => _BGMSource.volume = x, 0, 1.25f)
                        .OnComplete(() =>
                        {
                            if (_BGMSource.clip != null) _BGMSource.Stop();
                            _BGMSource.clip = BGMs.dataList[soundNumber];
                            _BGMSource.Play();
                            DOTween.To(() => 0, x => _BGMSource.volume = x, 1, 1.25f);
                        });
                }
                break;
        }
    }
    public void VoicePlay(AudioClip audio)
    {
        _voiceSource.Stop();
        _voiceSource.clip = audio;
        _voiceSource.Play();
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
                if (SaveDataManager._mainSaveData.health <= 0)
                {
                    SaveDataManager.Save(new SaveData(0, 0, 100, 80, 100, 0, WorldManager.Weather.sunny, Enumerable.Repeat(false, _storyManager._storyData.Count).ToList()));
                    StartCoroutine(SceneChange(SceneChanger.SceneKind.Title));
                    yield break;
                }
                break;
            case SceneChanger.SceneKind.Title:
                _pauseUI.HidePause();
                yield return SceneChanger.UnloadScene(SceneChanger.SceneKind.WorldManager);
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
