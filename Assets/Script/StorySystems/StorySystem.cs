using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StorySystem : MonoBehaviour
{
    [Header("�v���p�e�B")]
    [Tooltip("�e�L�X�g�X�s�[�h")]
    public float _textSpeed = 1;
    [SerializeField, Tooltip("�A���n�C���C�g�J���[")]
    Color _unHighLightColor = Color.white;

    [HideInInspector]
    bool _nextTextUpdating;

    //�R���|�[�l���g
    MainUI _mainUI;

    [Header("�I�u�W�F�N�g")]
    public List<StoryCharacterList> _characterList = new()
    {
        new StoryCharacterList {characterName = "System"},
        new StoryCharacterList { characterName = "Symphony" },
    };

    [Header("�e�L�X�g")]
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
        //CharacterNames�����Z�b�g����
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
                else Debug.LogWarning($"{character.gameObject.name}��Animator���A�^�b�`���Ă�������");

                if (character.gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
                {
                    spriteRenderer = sr;
                }
                else Debug.Log($"{character.gameObject.name}�ɂ�SpriteRenderer���A�^�b�`����Ă��܂���ł���");
            }
            else Debug.LogWarning($"characterList��{character.characterName}�ɃI�u�W�F�N�g���A�T�C�����Ă�������");


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

        //���̃e�L�X�g�ɂ���
        if (_textList.Count - 1 > _currentTextNumber)
        {
            _currentTextNumber++;
        }
        else
        {
            Debug.LogWarning("�e�L�X�g�͏I�����܂���");
            _nextTextUpdating = false;
            yield break;
        }

        //textList��kind�ɉ����ē�����ς���
        switch (_textList[_currentTextNumber].kind)
        {
            case StoryTextList.TextKind.move:

                //�Ώۂ�Animator��Play���\�b�h�𑗂�
                if (_characterList[_textList[_currentTextNumber].characterType].gameObject)
                {
                    if (_characterPropaties[_textList[_currentTextNumber].characterType].animator != null)
                    {
                        _characterPropaties[_textList[_currentTextNumber].characterType].animator.Play(_textList[_currentTextNumber].text);
                    }
                    else { Debug.LogWarning($"{_characterList[_textList[_currentTextNumber].characterType].gameObject.name}��Animator���A�^�b�`���Ă�������"); }
                }
                else { Debug.LogWarning($"characterList��{_characterList[_textList[_currentTextNumber].characterType].characterName}�ɃI�u�W�F�N�g���A�T�C�����Ă�������"); }

                StartCoroutine(WaitNextText());
                break;

            case StoryTextList.TextKind.text:
                if (_mainUI != null)
                {
                    //�����Ă���L�����݂̂��n�C���C�g����
                    CharacterHighLight(_textList[_currentTextNumber].characterType);
                    //textSpeed�̎��Ԃɉ����ăe�L�X�g��\������
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
                else Debug.LogWarning("mainUI���쐬���Ă�������");

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
