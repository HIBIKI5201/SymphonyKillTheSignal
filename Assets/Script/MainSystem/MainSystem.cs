using System.Collections;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    static MainSystem _selfInstance;

    StorySystem _storySystem;
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
            Debug.Log("u");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //UI ToolKitを取得
        _screenEffect = GetComponentInChildren<ScreenEffectUI>();
        _pauseUI = GetComponentInChildren<PauseUI>();

        //AudioSourceを取得する
        _soundEffectSource = GetComponentInChildren<AudioSource>();
        _BGMSource = GetComponent<AudioSource>();

        //テスト用
        StartCoroutine(StoryScene());
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

    IEnumerator StoryScene()
    {
        SceneChanger.ChangeScene(SceneChanger.SceneKind.Story);
        _screenEffect.ScreenEffect();
        yield return new WaitForSeconds(2);
        _storySystem = FindAnyObjectByType<StorySystem>();
        _storySystem._canselActive = true;
        _storySystem.NextTextTrigger();
    }
}
