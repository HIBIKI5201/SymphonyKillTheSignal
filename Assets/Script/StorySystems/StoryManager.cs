using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    StorySystem _storySystem;
    [SerializeField]
    List<StoryTextDataBase> _textDataBase = new();

    public void StoryStart()
    {
        _storySystem = FindAnyObjectByType<StorySystem>();
        _storySystem.SetClass(_textDataBase[0]);
        _storySystem.NextTextTrigger();
    }
}
