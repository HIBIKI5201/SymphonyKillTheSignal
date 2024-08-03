using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySystem : MonoBehaviour
{
    [Header("�v���p�e�B")]
    [SerializeField, Tooltip("�e�L�X�g�X�s�[�h")]
    float _textSpeed = 1;

    [HideInInspector]
    public bool _nextTextUpdating;

    MainUI _mainUI;

    [Header("�I�u�W�F�N�g")]
    public List<StoryCharacterList> _characterList = new()
    {
        new StoryCharacterList { characterName = "Symphony" },
    };

    [Header("�e�L�X�g")]
    [SerializeField]
    List<StoryTextList> _textList = new();

    readonly Dictionary<int, string> _characterNames = new();
    readonly Dictionary<int, Animator> _animators = new();

    int _currentTextNumber;

    private void Start()
    {
        _mainUI = FindAnyObjectByType<MainUI>();

        //CharacterNames�����Z�b�g����
        _characterNames.Clear();
        foreach (var character in _characterList)
        {
            _characterNames.Add(Array.IndexOf(_characterList.ToArray(), character), character.characterName);
        }

        //Animators�����Z�b�g����
        _animators.Clear();
        foreach (var character in _characterList)
        {
            if (character.gameObject)
            {
                if (character.gameObject.TryGetComponent<Animator>(out Animator animetor))
                {
                    _animators.Add(Array.IndexOf(_characterList.ToArray(), character), animetor);
                }
                else { Debug.LogWarning($"{character.gameObject.name}��Animator���A�^�b�`���Ă�������"); }
            }
            else { Debug.LogWarning($"characterList��{character.characterName}�ɃI�u�W�F�N�g���A�T�C�����Ă�������"); }
        }

        _currentTextNumber = 1;
    }

    public IEnumerator WaitNextText()
    {
        _nextTextUpdating = true;

        //���̃e�L�X�g�ɂ���
        if (_textList.Count - 1 > _currentTextNumber)
        {
            _currentTextNumber++;
        }
        else { Debug.LogWarning("�e�L�X�g�͏I�����܂���"); }

        //textList��kind�ɉ����ē�����ς���
        switch (_textList[_currentTextNumber].kind)
        {
            case StoryTextList.TextKind.move:

                //�Ώۂ�Animator��Play���\�b�h�𑗂�\��
                if (_characterList[_textList[_currentTextNumber].characterType].gameObject)
                {
                    if (_animators[_textList[_currentTextNumber].characterType] != null)
                    {
                        _animators[_textList[_currentTextNumber].characterType].Play(_textList[_currentTextNumber].text);
                    }
                    else { Debug.LogWarning($"{_characterList[_textList[_currentTextNumber].characterType].gameObject.name}��Animator���A�^�b�`���Ă�������"); }
                }
                else { Debug.LogWarning($"characterList��{_characterList[_textList[_currentTextNumber].characterType].characterName}�ɃI�u�W�F�N�g���A�T�C�����Ă�������"); }

                StartCoroutine(WaitNextText());
                break;

            case StoryTextList.TextKind.text:
                if (_mainUI != null)
                {
                    //textSpeed�̎��Ԃɉ����ăe�L�X�g��\������
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
                else { Debug.LogWarning("mainUI���쐬���Ă�������"); }


                _nextTextUpdating = false;
                break;
        }
    }
}
