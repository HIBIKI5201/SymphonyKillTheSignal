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
        //UI ToolKit���擾
        _screenEffect = GetComponentInChildren<ScreenEffectUI>();
        StartCoroutine(GameStartEffect());
        _pauseUI = GetComponentInChildren<PauseUI>();
        //�}�l�[�W���[�N���X���擾
        _storyManager = GetComponentInChildren<StoryManager>();
        _userDataManager = GetComponentInChildren<UserDataManager>();
        //AudioSource���擾����
        _soundEffectSource = GetComponentInChildren<AudioSource>();
        _BGMSource = GetComponent<AudioSource>();
        //�|�[�YUI���\��
        if (SceneChanger.CurrentScene == SceneChanger.SceneKind.Title)
        {
            _pauseUI.HidePause();
        }
        //�Z�[�u�f�[�^���m�F
        SaveDataManager._mainSaveData = SaveDataManager.Load();
        //�V�[���̃V�X�e�����N��
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
        //��������{�^�����Z�[�u�f�[�^������ꍇ
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
                Debug.LogWarning("SoundPlay���\�b�h�͈̔͊O�ł�");
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
        //�{�^�����b�N���N��
        _screenEffect.ButtonUnactiveElement(true);
        //�t�F�[�h�A�E�g���o
        _screenEffect.ScreenFadeOut(1);
        yield return new WaitForSeconds(1);
        //�V�[�������[�h���ă��[�h�I���܂ő҂�
        yield return SceneChanger.ChangeScene(sceneKind);
        //�V�X�e���n��������
        FindAnyObjectByType<SystemBase>()?.SystemAwake(this);
        //���[�h�����V�[���ɉ����ē�����ς���
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
        //�t�F�[�h�C�����o
        _screenEffect.ScreenFadeIn(1.5f);
        yield return new WaitForSeconds(1.5f);
        //�{�^�����b�N������
        _screenEffect.ButtonUnactiveElement(false);
    }
}
