using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    StorySystem _storySystem;
    ScreenEffectUI _screenEffect;

    void Start()
    {
        _storySystem = FindAnyObjectByType<StorySystem>();
        _screenEffect = GetComponentInChildren<ScreenEffectUI>();

        StartCoroutine(StoryScene());
    }


    void Update()
    {
        
    }

    IEnumerator StoryScene()
    {
        _screenEffect.ScreenEffect();
        yield return new WaitForSeconds(1);
        _storySystem._canselActive = true;
        _storySystem.NextTextTrigger();
    }
}
