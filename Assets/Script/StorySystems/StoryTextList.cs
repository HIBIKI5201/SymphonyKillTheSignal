using System;

[Serializable]
public class StoryTextList
{
    public enum TextKind
    {
        text,
        move,
        effect
    }

    public int characterType;
    public TextKind kind;
    public string text;
}