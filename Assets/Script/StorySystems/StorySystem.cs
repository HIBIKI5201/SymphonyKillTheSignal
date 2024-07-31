using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorySystem : MonoBehaviour
{
    [SerializeField]
    Text textBox;

    [Header("オブジェクト")]
    public List<StoryCharacterList> characterList = new()
    {
        new StoryCharacterList { characterName = "effect"},
        new StoryCharacterList { characterName = "Symphony" },
    };

    [SerializeField]
    List<StoryTextList> textList = new();

    readonly Dictionary<int, string> CharacterNames = new();
    readonly Dictionary<int, Animator> Animators = new();

    int textNumber;

    private void Start()
    {
        //CharacterNamesをリセットする
        CharacterNames.Clear();
        foreach (var character in characterList)
        {
            CharacterNames.Add(Array.IndexOf(characterList.ToArray(), character), character.characterName);
        }

        //Animatorsをリセットする
        Animators.Clear();
        foreach (var character in characterList)
        {
            if (character.gameObject)
            {
                if (character.gameObject.TryGetComponent<Animator>(out Animator animetor))
                {
                    Animators.Add(Array.IndexOf(characterList.ToArray(), character), animetor);
                }
                else { Debug.LogWarning($"{character.gameObject.name}にAnimatorをアタッチしてください"); }
            }
            else { Debug.LogWarning($"{character}にオブジェクトをアサインしてください"); }
        }
    }

    public void NextText()
    {
        // 喋っているキャラクターの名前を取得
        Debug.Log(CharacterNames[textList[textNumber].characterType]);

        //textListのkindに応じて動きを変える
        switch (textList[textNumber].kind)
        {
            case StoryTextList.TextKind.text:

                //|を書くと改行する
                string[] texts = textList[textNumber].text.Split('|');
                string text = "";

                foreach (var oneText in texts)
                {
                    text += oneText;
                    text += "\n";
                }

                Debug.Log(text);
                if (textBox)
                {
                    textBox.text = text;
                }
                else { Debug.LogWarning("テキストボックスをアサインしてください"); }

                break;


            case StoryTextList.TextKind.move:

                //対象のAnimatorにPlayメソッドを送る予定
                Animators[textList[textNumber].characterType].Play(textList[textNumber].text);

                break;


            case StoryTextList.TextKind.effect:

                //未完成

                break;
        }

        //次のテキストにする
        if (textList.Count - 1 > textNumber)
        {
            textNumber++;
        }
        else { Debug.LogWarning("テキストは終了しました"); }
    }
}
