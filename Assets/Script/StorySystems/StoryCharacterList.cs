using System;
using UnityEngine;

[Serializable]
public class StoryCharacterList
{
    public GameObject gameObject;
    public string characterName;

    public StoryCharacterList()
    {

    }
    public StoryCharacterList(StoryCharacterList other)
    {
        gameObject = other.gameObject;
        characterName = other.characterName;
    }
}