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
        //Singleton�̂悤�ɂ��鏈��
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
        //UI ToolKit���擾
        _screenEffect = GetComponentInChildren<ScreenEffectUI>();
        _pauseUI = GetComponentInChildren<PauseUI>();
        //StoryManager���擾
        _storyManager = GetComponentInChildren<StoryManager>();
        //AudioSource���擾����
        _soundEffectSource = GetComponentInChildren<AudioSource>();
        _BGMSource = GetComponent<AudioSource>();
        //�|�[�YUI���\��
        if (SceneChanger.CurrentScene == SceneChanger.SceneKind.Title)
        {
            _pauseUI.enabled = false;
        }
        //�Z�[�u�f�[�^���m�F
        SaveDataManager._mainSaveData = SaveDataManager.Load();
    }

    public void GameStart(bool Continue)
    {
        //��������{�^�����Z�[�u�f�[�^������ꍇ
        if (Continue && SaveDataManager._mainSaveData.HasValue)
        {
            StartCoroutine(SceneChange(SceneChanger.SceneKind.Story));
        }
        else
        {
            StartCoroutine(SceneChange(SceneChanger.SceneKind.Home));
            SaveDataManager._mainSaveData = new(0, 0);
            SaveDataManager.Save();
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
                Debug.LogWarning("SoundPlay���\�b�h�͈̔͊O�ł�");
                break;
        }
    }

    IEnumerator SceneChange(SceneChanger.SceneKind sceneKind)
    {
        //�t�F�[�h�A�E�g���o
        _screenEffect.ScreenFadeOut();
        yield return new WaitForSeconds(1);
        //�V�[�������[�h���ă��[�h�I���܂ő҂�
        AsyncOperation asyncLoad = SceneChanger.ChangeScene(sceneKind);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        //���[�h�����V�[���ɉ����ē�����ς���
        switch (sceneKind)
        {
            case SceneChanger.SceneKind.Story:
                _pauseUI.enabled = true;
                _storyManager.StoryStart();
                break;
            case SceneChanger.SceneKind.Home:
                _pauseUI.enabled = true;
                break;
        }
        //�t�F�[�h�C�����o
        _screenEffect.ScreenFadeIn();
    }
}
