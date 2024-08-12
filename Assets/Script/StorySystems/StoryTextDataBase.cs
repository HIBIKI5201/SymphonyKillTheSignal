using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StoryTextDataBase : ScriptableObject
{
    [Header("�I�u�W�F�N�g")]
    public List<StoryCharacterList> _characterList = new()
    {
        new StoryCharacterList {characterName = "System"},
        new StoryCharacterList { characterName = "Symphony" },
    };

    [Header("�e�L�X�g")]
    public List<StoryTextList> _textList = new();
}
