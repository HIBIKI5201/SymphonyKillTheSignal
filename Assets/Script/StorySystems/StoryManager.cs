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
        _storySystem.TextDataLoad(_textDataBase[0]);
        //�ŏ��̃e�L�X�g���Ăяo��
        _storySystem.NextTextTrigger();
        _storySystem._textUpdateActive = true;
    }
}
