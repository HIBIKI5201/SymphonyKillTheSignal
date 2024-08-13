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

    [HideInInspector]
    public List<StoryCharacterList> _characterList = new();
    List<StoryTextList> _textList;


    public struct CharacterPropaty
    {
        public string name;
        public Animator animator;
        public CharacterRendererManager characterRendererManager;

        public CharacterPropaty(string name, Animator animator, CharacterRendererManager sprite)
        {
            this.name = name;
            this.animator = animator;
            this.characterRendererManager = sprite;
        }
    }

    readonly Dictionary<int, CharacterPropaty> _characterPropaties = new();

    int _currentTextNumber;
    bool _textUpdatingCanselTrigger;
    public bool _textUpdateActive;
    Coroutine _nextTimerCoroutine;

    private void Start()
    {
        //メンバーの初期設定
        _nextTextUpdating = false;
        //UIのPresenterを取得
        _mainUI = FindAnyObjectByType<StoryUI>();
        //最初が0番目のテキストになるように初期値を設定
        _currentTextNumber = -1;
    }

    public void SetClass(StoryTextDataBase storyTextData)
    {
        //メインシステムを取得
        _mainSystem = FindAnyObjectByType<MainSystem>();
        //Characterをデータベースから値渡し
        _characterList.Clear();
        foreach (StoryCharacterList character in storyTextData._characterList)
        {
            if (character.characterName == "System")
            {
                _characterList.Add(new StoryCharacterList { gameObject = this.gameObject, characterName = "" });
                continue;
            }
            _characterList.Add(new StoryCharacterList(character));
        }
        //Textをデータベースから参照
        _textList = new List<StoryTextList> (storyTextData._textList);
        //CharacterPropatiesをリセットする
        _characterPropaties.Clear();
        foreach (var characterData in _characterList)
        {
            string characterName = characterData.characterName;
            GameObject character = null;
            //データからゲームオブジェクトを生成
            if (characterData.characterName != "System")
            {
                character = Instantiate(characterData.gameObject);
                character.transform.SetParent(gameObject.transform);
            }
            Animator animator = null;
            CharacterRendererManager spriteRenderer = null;
            //各キャラクターからAnimatorとSpriteRendererを取得する
            if (character)
            {
                if (character.TryGetComponent<Animator>(out Animator anim))
                {
                    animator = anim;
                }
                else Debug.LogWarning($"{character.name}にAnimatorをアタッチしてください");

                if (character.TryGetComponent<CharacterRendererManager>(out CharacterRendererManager crm))
                {
                    spriteRenderer = crm;
                }
                else Debug.Log($"{character.name}にはCharacterRenererManagerがアタッチされていませんでした");
            }
            else Debug.LogWarning($"characterListの{characterData.characterName}にオブジェクトをアサインしてください");
            //取得したコンポーネントをリストに格納する
            _characterPropaties.Add(Array.IndexOf(_characterList.ToArray(), characterData), new CharacterPropaty(characterName, animator, spriteRenderer));
        }
        //最初のテキストを呼び出す
        _textUpdateActive = true;
    }
    /// <summary>
    /// テキストボックスが押された時に実行されるイベント
    /// </summary>
    /// <param name="context"></param>
    public void NextPageButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && _textList != null)
        {
            NextTextTrigger();
        }
    }
    /// <summary>
    /// 次のテキストを呼び出す
    /// </summary>
    public void NextTextTrigger()
    {
        //キ
        if (_textUpdateActive)
        {
            StartCoroutine(NextText());
        }
    }
    /// <summary>
    /// 次のテキストを呼び出せるまでのタイマーを起動するメソッド
    /// </summary>
    /// <param name="time">タイマーのセット時間</param>
    /// <returns></returns>
    IEnumerator NextTimer(float time)
    {
        _textUpdateActive = false;
        yield return new WaitForSeconds(time);
        _textUpdateActive = true;
    }
    /// <summary>
    /// 次のテキストを呼び出す処理
    /// </summary>
    /// <returns></returns>
    IEnumerator NextText()
    {
        //テキスト更新中にボタンが押された場合にテキストを最後まで表示するトリガーを起動する
        if (_nextTextUpdating && _textList[_currentTextNumber].kind == StoryTextList.TextKind.text)
        {
            _textUpdatingCanselTrigger = true;
            yield break;
        }
        //次のテキストにする
        if (_textList.Count - 1 > _currentTextNumber)
        {
            _currentTextNumber++;
        }
        //次のテキストがない場合は処理を終わる
        else
        {
            Debug.LogWarning("テキストは終了しました");
            yield break;
        }
        _nextTextUpdating = true;
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
                float waitTime = 1;
                if (texts.Length > 1)
                {
                    waitTime = float.Parse(texts[1]);
                }
                //アニメーション中はテキスト更新を無効
                if (_nextTimerCoroutine != null)
                {
                    StopCoroutine(_nextTimerCoroutine);
                }
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
                        //もしCanselTriggerがあれば途中で更新を止める
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
                //テキスト更新の処理を終わる
                _nextTextUpdating = false;
                break;

            case StoryTextList.TextKind.sound:
                //文字列を数値化してSoundPlayメソッドを送る
                if (int.TryParse(texts[0], out int num) && int.TryParse(texts[1], out int soundNum))
                {
                    _mainSystem.SoundPlay(num, soundNum);
                }
                else Debug.LogWarning("Soundの指定が適切ではありませんでした");
                break;
        }
    }

    void CharacterHighLight(int number)
    {
        for (int i = 0; i < _characterPropaties.Count; i++)
        {
            if (_characterPropaties[i].characterRendererManager != null)
                _characterPropaties[i].characterRendererManager.ChangeColor(i != number ? _unHighLightColor : Color.white);
        }
    }
}
