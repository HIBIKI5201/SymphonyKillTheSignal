using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StorySystem : MonoBehaviour
{
    [Header("プロパティ")]
    [Tooltip("テキストスピード")]
    public float _textSpeed = 1;
    [SerializeField, Tooltip("アンハイライトカラー")]
    Color _unHighLightColor = Color.white;

    [HideInInspector]
    bool _nextTextUpdating;

    //コンポーネント
    MainUI _mainUI;

    [Header("オブジェクト")]
    public List<StoryCharacterList> _characterList = new()
    {
        new StoryCharacterList {characterName = "System"},
        new StoryCharacterList { characterName = "Symphony" },
    };

    [Header("テキスト")]
    [SerializeField]
    List<StoryTextList> _textList = new();

    public struct CharacterPropaty
    {
        public string name;
        public Animator animator;
        public SpriteRenderer spriteRenderer;

        public CharacterPropaty(string name, Animator animator, SpriteRenderer sprite)
        {
            this.name = name;
            this.animator = animator;
            this.spriteRenderer = sprite;
        }
    }

    readonly Dictionary<int, CharacterPropaty> _characterPropaties = new();

    int _currentTextNumber;
    bool _textUpdatingCanselTrigger;
    bool _canselActive = true;

    private void Start()
    {
        _nextTextUpdating = false;
        _mainUI = FindAnyObjectByType<MainUI>();
        //CharacterNamesをリセットする
        _characterPropaties.Clear();
        foreach (var character in _characterList)
        {
            string characterName = character.characterName;
            Animator animator = null;
            SpriteRenderer spriteRenderer = null;
            if (character.gameObject)
            {
                if (character.gameObject.TryGetComponent<Animator>(out Animator anim))
                {
                    animator = anim;
                }
                else Debug.LogWarning($"{character.gameObject.name}にAnimatorをアタッチしてください");

                if (character.gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
                {
                    spriteRenderer = sr;
                }
                else Debug.Log($"{character.gameObject.name}にはSpriteRendererがアタッチされていませんでした");
            }
            else Debug.LogWarning($"characterListの{character.characterName}にオブジェクトをアサインしてください");


            _characterPropaties.Add(Array.IndexOf(_characterList.ToArray(), character), new CharacterPropaty(characterName, animator, spriteRenderer));
        }
        _currentTextNumber = -1;
    }

    public void NextPageButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            NextTextTrigger();
        }
    }

    public void NextTextTrigger()
    {
        if (_canselActive)
        {
            StartCoroutine(WaitNextText());
            StartCoroutine(NextTimer(0.1f));
        }
    }

    IEnumerator WaitNextText()
    {
        if (_nextTextUpdating && _textList[_currentTextNumber].kind == StoryTextList.TextKind.text)
        {
            _textUpdatingCanselTrigger = true;
            yield break;
        }

        _nextTextUpdating = true;

        //次のテキストにする
        if (_textList.Count - 1 > _currentTextNumber)
        {
            _currentTextNumber++;
        }
        else
        {
            Debug.LogWarning("テキストは終了しました");
            _nextTextUpdating = false;
            yield break;
        }

        //textListのkindに応じて動きを変える
        switch (_textList[_currentTextNumber].kind)
        {
            case StoryTextList.TextKind.move:

                //対象のAnimatorにPlayメソッドを送る
                if (_characterList[_textList[_currentTextNumber].characterType].gameObject)
                {
                    if (_characterPropaties[_textList[_currentTextNumber].characterType].animator != null)
                    {
                        _characterPropaties[_textList[_currentTextNumber].characterType].animator.Play(_textList[_currentTextNumber].text);
                    }
                    else { Debug.LogWarning($"{_characterList[_textList[_currentTextNumber].characterType].gameObject.name}にAnimatorをアタッチしてください"); }
                }
                else { Debug.LogWarning($"characterListの{_characterList[_textList[_currentTextNumber].characterType].characterName}にオブジェクトをアサインしてください"); }

                StartCoroutine(WaitNextText());
                break;

            case StoryTextList.TextKind.text:
                if (_mainUI != null)
                {
                    //喋っているキャラのみをハイライトする
                    CharacterHighLight(_textList[_currentTextNumber].characterType);
                    //textSpeedの時間に応じてテキストを表示する
                    float x = 0;
                    do
                    {
                        x += _textSpeed * Time.deltaTime;

                        _mainUI.TextBoxUpdate(
                            _characterPropaties[_textList[_currentTextNumber].characterType].name,
                            _textList[_currentTextNumber].text.Substring(0, Mathf.Min((int)x, _textList[_currentTextNumber].text.Length)));
                        if (_textUpdatingCanselTrigger)
                        {
                            _mainUI.TextBoxUpdate(_characterPropaties[_textList[_currentTextNumber].characterType].name, _textList[_currentTextNumber].text);
                            _textUpdatingCanselTrigger = false;
                            break;
                        }
                        yield return new WaitForEndOfFrame();
                    } while (x < _textList[_currentTextNumber].text.Length);


                }
                else Debug.LogWarning("mainUIを作成してください");

                _nextTextUpdating = false;
                break;
        }
    }

    IEnumerator NextTimer(float time)
    {
        _canselActive = false;
        yield return new WaitForSeconds(time);
        _canselActive = true;
    }

    void CharacterHighLight(int number)
    {
        for (int i = 0; i < _characterPropaties.Count; i++)
        {
            if (_characterPropaties[i].spriteRenderer != null)
                _characterPropaties[i].spriteRenderer.color = i != number ? _unHighLightColor : Color.white;
        }
    }
}
