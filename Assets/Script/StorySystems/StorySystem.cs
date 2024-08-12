using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StorySystem : MonoBehaviour
{
    [Header("プロパティ")]
    [Tooltip("テキストスピード")]
    public static float _textSpeed = 15;
    [SerializeField, Tooltip("アンハイライトカラー")]
    Color _unHighLightColor = Color.white;

    [HideInInspector]
    bool _nextTextUpdating;

    //クラス
    MainSystem _mainSystem;
    StoryUI _mainUI;

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
    public bool _canselActive;
    Coroutine _nextTimerCoroutine;

    private void Start()
    {
        _mainSystem = FindAnyObjectByType<MainSystem>();
        _nextTextUpdating = false;
        _mainUI = FindAnyObjectByType<StoryUI>();
        //CharacterNamesをリセットする
        _characterPropaties.Clear();
        foreach (var character in _characterList)
        {
            string characterName = character.characterName;
            Animator animator = null;
            SpriteRenderer spriteRenderer = null;
            //各キャラクターからAnimatorとSpriteRendererを取得する
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
            //取得したコンポーネントをリストに格納する
            _characterPropaties.Add(Array.IndexOf(_characterList.ToArray(), character), new CharacterPropaty(characterName, animator, spriteRenderer));
        }
        //最初が0番目のテキストになるように初期値を設定
        _currentTextNumber = -1;

        _canselActive = false;
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
            StartCoroutine(NextText());
        }
    }

    IEnumerator NextTimer(float time)
    {
        _canselActive = false;
        yield return new WaitForSeconds(time);
        _canselActive = true;
    }

    IEnumerator NextText()
    {
        //テキスト更新中にボタンが押された場合にテキストを最後まで表示するトリガーを起動する
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
        //改行ごとに文字を分ける
        string[] texts = _textList[_currentTextNumber].text.Split('\n');
        //textListのkindに応じて動きを変える
        switch (_textList[_currentTextNumber].kind)
        {
            case StoryTextList.TextKind.move:
                //動いているキャラのみをハイライトする
                CharacterHighLight(_textList[_currentTextNumber].characterType);
                //対象のAnimatorにPlayメソッドを送る
                if (_characterList[_textList[_currentTextNumber].characterType].gameObject)
                {
                    if (_characterPropaties[_textList[_currentTextNumber].characterType].animator != null)
                    {
                        _characterPropaties[_textList[_currentTextNumber].characterType].animator.Play(texts[0]);
                    }
                    else { Debug.LogWarning($"{_characterList[_textList[_currentTextNumber].characterType].gameObject.name}にAnimatorをアタッチしてください"); }
                }
                else { Debug.LogWarning($"characterListの{_characterList[_textList[_currentTextNumber].characterType].characterName}にオブジェクトをアサインしてください"); }
                //アニメーションの待機時間を計算
                int waitTime = 1;
                if (texts.Length > 1)
                {
                    waitTime = int.Parse(texts[1]);
                }
                //アニメーション中はテキスト更新を無効
                StopCoroutine(_nextTimerCoroutine);
                _nextTimerCoroutine = StartCoroutine(NextTimer(waitTime));
                yield return new WaitForSeconds(waitTime);
                StartCoroutine(NextText());
                break;

            case StoryTextList.TextKind.text:
                if (_mainUI != null)
                {
                    //連打防止のためのタイマー
                    _nextTimerCoroutine = StartCoroutine(NextTimer(0.1f));
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

            case StoryTextList.TextKind.sound:
                if (int.TryParse(texts[0], out int num) && int.TryParse(texts[1], out int soundNum))
                {
                    _mainSystem.SoundPlay(num, soundNum);
                }
                break;
        }
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
