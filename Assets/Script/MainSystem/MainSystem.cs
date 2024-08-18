using System.Collections;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    static MainSystem _selfInstance;
    [HideInInspector]
    public StoryManager _storyManager;

    ScreenEffectUI _screenEffect;
    PauseUI _pauseUI;

    AudioSource _soundEffectSource;
    AudioSource _BGMSource;
    [SerializeField]
    SoundDataBase soundEffects;
    [SerializeField]
    SoundDataBase BGMs;
    private void Awake()
    {

    }

    void Start()
    {
        //UI ToolKitを取得
        _screenEffect = GetComponentInChildren<ScreenEffectUI>();
        _screenEffect.ButtonUnactiveElement(false);
        _pauseUI = GetComponentInChildren<PauseUI>();
        //StoryManagerを取得
        _storyManager = GetComponentInChildren<StoryManager>();
        //AudioSourceを取得する
        _soundEffectSource = GetComponentInChildren<AudioSource>();
        _BGMSource = GetComponent<AudioSource>();
        //ポーズUIを非表示
        if (SceneChanger.CurrentScene == SceneChanger.SceneKind.Title)
        {
            _pauseUI.HidePause();
            Debug.Log("ポーズを非表示");
        }
        //セーブデータを確認
        SaveDataManager._mainSaveData = SaveDataManager.Load();
        //シーンのシステムを起動
        FindAnyObjectByType<SystemBase>().SystemAwake(this);
    }

    public void GameStart(bool Continue)
    {
        //続きからボタンかつセーブデータがある場合
        if (Continue && SaveDataManager._mainSaveData.HasValue)
        {
            StartCoroutine(SceneChange(SceneChanger.SceneKind.Home));
        }
        else
        {
            StartCoroutine(SceneChange(SceneChanger.SceneKind.Story));
            SaveDataManager._mainSaveData = new(0, 0);
            SaveDataManager.Save();
        }
    }

    public void BackToHome()
    {
        StartCoroutine(SceneChange(SceneChanger.SceneKind.Home));
    }

    public void BackToTitle()
    {
        StartCoroutine(SceneChange(SceneChanger.SceneKind.Title));
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
                _storyManager.StoryStart();
                break;
            case SceneChanger.SceneKind.Home:
                _pauseUI.RevealPause();
                break;
            case SceneChanger.SceneKind.Title:
                _pauseUI.HidePause();
                break;
        }
        //フェードイン演出
        yield return new WaitForSeconds(0.8f);
        _screenEffect.ScreenFadeIn(1);
        //ボタンロックを解除
        _screenEffect.ButtonUnactiveElement(false);
    }
}
