using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StoryTextDataBase : ScriptableObject
{
    [Header("オブジェクト")]
    public List<StoryCharacterList> _characterList = new()
    {
        new StoryCharacterList {characterName = "System"},
        new StoryCharacterList { characterName = "Symphony" },
    };

    [Header("テキスト")]
    public List<StoryTextList> _textList = new();
}
