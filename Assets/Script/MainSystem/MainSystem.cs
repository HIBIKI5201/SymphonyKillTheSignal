using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    MainSystem _selfInstance;

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
        //SingletonÇÃÇÊÇ§Ç…Ç∑ÇÈèàóù
        if (_selfInstance == null)
        {
            _selfInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        //UI ToolKitÇéÊìæ
        _screenEffect = GetComponentInChildren<ScreenEffectUI>();
        _pauseUI = GetComponentInChildren<PauseUI>();

        //AudioSourceÇéÊìæÇ∑ÇÈ
        _soundEffectSource = GetComponentInChildren<AudioSource>();
        _BGMSource = GetComponent<AudioSource>();

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
        _storySystem = FindAnyObjectByType<StorySystem>();

        _screenEffect.ScreenEffect();
        yield return new WaitForSeconds(2);
        _storySystem._canselActive = true;
        _storySystem.NextTextTrigger();
    }
}
