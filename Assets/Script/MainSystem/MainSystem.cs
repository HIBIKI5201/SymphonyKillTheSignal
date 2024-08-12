using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    MainSystem _selfInstance;

    StorySystem _storySystem;
    ScreenEffectUI _screenEffect;

    private void Awake()
    {
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
        _screenEffect = GetComponentInChildren<ScreenEffectUI>();

        StartCoroutine(StoryScene());
    }


    void Update()
    {
        
    }

    IEnumerator StoryScene()
    {
        _storySystem = FindAnyObjectByType<StorySystem>();

        _screenEffect.ScreenEffect();
        yield return new WaitForSeconds(1);
        _storySystem._canselActive = true;
        _storySystem.NextTextTrigger();
    }
}
