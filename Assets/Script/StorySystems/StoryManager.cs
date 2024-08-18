using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    StorySystem _storySystem;
    [SerializeField]
    List<StoryTextDataBase> _textDataBase = new();

    public void SetStoryData()
    {
        _storySystem = FindAnyObjectByType<StorySystem>();
        _storySystem.TextDataLoad(_textDataBase[0]);
    }

    public void StartStory()
    {
        //最初のテキストを呼び出す
        StartCoroutine(_storySystem.NextText());
    }
}
