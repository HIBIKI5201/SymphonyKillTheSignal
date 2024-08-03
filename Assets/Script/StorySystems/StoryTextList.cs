using System;
using UnityEngine;

[Serializable]
public class StoryTextList
{
    public enum TextKind
    {
        text,
        move,
    }

    public int characterType;
    public TextKind kind;
    [TextArea]
    public string text;
}