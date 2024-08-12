using System.Collections;
using System.Collections.Generic;
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
    List<AudioClip> soundEffects = new();
    [SerializeField]
    List<AudioClip> BGMs = new();
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
            Debug.Log("u");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //UI ToolKit���擾
        _screenEffect = GetComponentInChildren<ScreenEffectUI>();
        _pauseUI = GetComponentInChildren<PauseUI>();

        //AudioSource���擾����
        _soundEffectSource = GetComponentInChildren<AudioSource>();
        _BGMSource = GetComponent<AudioSource>();

        //�e�X�g�p
        StartCoroutine(StoryScene());
    }

    public void SoundPlay(int number, int soundNumber)
    {
        switch (number)
        {
            case 0:
                _soundEffectSource.PlayOneShot(soundEffects[soundNumber]);
                break;

            case 1:
                _BGMSource.Stop();
                _BGMSource.clip = BGMs[soundNumber];
                _BGMSource.Play();
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
