using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySystem : MonoBehaviour
{
    [Header("プロパティ")]
    [SerializeField, Tooltip("テキストスピード")]
    float _textSpeed = 1;

    [HideInInspector]
    public bool _nextTextUpdating;

    MainUI _mainUI;

    [Header("オブジェクト")]
    public List<StoryCharacterList> _characterList = new()
    {
        new StoryCharacterList { characterName = "Symphony" },
    };

    [Header("テキスト")]
    [SerializeField]
    List<StoryTextList> _textList = new();

    readonly Dictionary<int, string> _characterNames = new();
    readonly Dictionary<int, Animator> _animators = new();

    int _currentTextNumber;

    private void Start()
    {
        _mainUI = FindAnyObjectByType<MainUI>();

        //CharacterNamesをリセットする
        _characterNames.Clear();
        foreach (var character in _characterList)
        {
            _characterNames.Add(Array.IndexOf(_characterList.ToArray(), character), character.characterName);
        }

        //Animatorsをリセットする
        _animators.Clear();
        foreach (var character in _characterList)
        {
            if (character.gameObject)
            {
                if (character.gameObject.TryGetComponent<Animator>(out Animator animetor))
                {
                    _animators.Add(Array.IndexOf(_characterList.ToArray(), character), animetor);
                }
                else { Debug.LogWarning($"{character.gameObject.name}にAnimatorをアタッチしてください"); }
            }
            else { Debug.LogWarning($"characterListの{character.characterName}にオブジェクトをアサインしてください"); }
        }

        _currentTextNumber = 1;
    }

    public IEnumerator WaitNextText()
    {
        _nextTextUpdating = true;

        //次のテキストにする
        if (_textList.Count - 1 > _currentTextNumber)
        {
            _currentTextNumber++;
        }
        else { Debug.LogWarning("テキストは終了しました"); }

        //textListのkindに応じて動きを変える
        switch (_textList[_currentTextNumber].kind)
        {
            case StoryTextList.TextKind.move:

                //対象のAnimatorにPlayメソッドを送る予定
                if (_characterList[_textList[_currentTextNumber].characterType].gameObject)
                {
                    if (_animators[_textList[_currentTextNumber].characterType] != null)
                    {
                        _animators[_textList[_currentTextNumber].characterType].Play(_textList[_currentTextNumber].text);
                    }
                    else { Debug.LogWarning($"{_characterList[_textList[_currentTextNumber].characterType].gameObject.name}にAnimatorをアタッチしてください"); }
                }
                else { Debug.LogWarning($"characterListの{_characterList[_textList[_currentTextNumber].characterType].characterName}にオブジェクトをアサインしてください"); }

                StartCoroutine(WaitNextText());
                break;

            case StoryTextList.TextKind.text:
                if (_mainUI != null)
                {
                    //textSpeedの時間に応じてテキストを表示する
                    float x = 0;
                    do
                    {
                        x += _textList[_currentTextNumber].text.Length / _textSpeed * Time.deltaTime;

                        _mainUI.TextBoxUpdate(
                            _characterNames[_textList[_currentTextNumber].characterType],
                            _textList[_currentTextNumber].text.Substring(0, Mathf.Min((int)x, _textList[_currentTextNumber].text.Length)));

                        yield return new WaitForEndOfFrame();
                    } while (x >= _textList[_currentTextNumber].text.Length);

                    
                }
                else { Debug.LogWarning("mainUIを作成してください"); }


                _nextTextUpdating = false;
                break;
        }
    }
}
