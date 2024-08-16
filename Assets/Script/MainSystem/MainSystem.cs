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
        //Singletonのようにする処理
        if (_selfInstance == null)
        {
            _selfInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //UI ToolKitを取得
        _screenEffect = GetComponentInChildren<ScreenEffectUI>();
        _pauseUI = GetComponentInChildren<PauseUI>();
        //StoryManagerを取得
        _storyManager = GetComponentInChildren<StoryManager>();
        //AudioSourceを取得する
        _soundEffectSource = GetComponentInChildren<AudioSource>();
        _BGMSource = GetComponent<AudioSource>();
    }

    public void GameStart(bool Continue)
    {
        SaveDataManager.SaveData? saveData =　SaveDataManager.Load();
        //続きからボタンかつセーブデータがある場合
        if (Continue && saveData.HasValue)
        {
            StartCoroutine(SceneChange(SceneChanger.SceneKind.Story));
            SaveDataManager._mainSaveData = saveData.Value;
        }
        else
        {
            StartCoroutine(SceneChange(SceneChanger.SceneKind.Home));
            SaveDataManager.Save(new SaveDataManager.SaveData(0, 0));
        }
    }

    public void BackToHome()
    {
        StartCoroutine(SceneChange(SceneChanger.SceneKind.Home));
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
        //フェードアウト演出
        _screenEffect.ScreenFadeOut();
        yield return new WaitForSeconds(1);
        //シーンをロードしてロード終了まで待つ
        AsyncOperation asyncLoad = SceneChanger.ChangeScene(sceneKind);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        //ロードしたシーンに応じて動きを変える
        switch(sceneKind)
        {
            case SceneChanger.SceneKind.Story:
        _storyManager.StoryStart();
                break;
        }
        //フェードイン演出
        _screenEffect.ScreenFadeIn();
    }
}
