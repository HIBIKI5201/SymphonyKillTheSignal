using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StorySystem : MonoBehaviour
{
    [Header("�v���p�e�B")]
    [Tooltip("�e�L�X�g�X�s�[�h")]
    public static float _textSpeed = 15;
    [SerializeField, Tooltip("�A���n�C���C�g�J���[")]
    Color _unHighLightColor = Color.white;

    [HideInInspector]
    bool _nextTextUpdating;

    //�N���X
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
        //�����o�[�̏����ݒ�
        _nextTextUpdating = false;
        //UI��Presenter���擾
        _mainUI = FindAnyObjectByType<StoryUI>();
        //�ŏ���0�Ԗڂ̃e�L�X�g�ɂȂ�悤�ɏ����l��ݒ�
        _currentTextNumber = -1;
    }

    public void SetClass(StoryTextDataBase storyTextData)
    {
        //���C���V�X�e�����擾
        _mainSystem = FindAnyObjectByType<MainSystem>();
        //Character���f�[�^�x�[�X����l�n��
        _characterList.Clear();
        foreach (StoryCharacterList character in storyTextData._characterList)
        {
            //System�̂ݗ�O����
            if (character.characterName == "System")
            {
                _characterList.Add(new StoryCharacterList { gameObject = this.gameObject, characterName = "", pos = Vector2.zero });
                continue;
            }
            //�f�[�^���R�s�[
            _characterList.Add(new StoryCharacterList(character));
        }
        //Text���f�[�^�x�[�X����Q��
        _textList = new List<StoryTextList>(storyTextData._textList);
        //CharacterPropaties�����Z�b�g����
        _characterPropaties.Clear();
        //�L�����N�^�[�𐶐�
        foreach (var characterData in _characterList)
        {
            string characterName = characterData.characterName;
            GameObject character = null;
            if (characterData.characterName != "System")
            {
                //�f�[�^����Q�[���I�u�W�F�N�g�𐶐�
                character = Instantiate(characterData.gameObject);
                //StorySystem�̎q�I�u�W�F�N�g�ɂ���
                character.transform.SetParent(gameObject.transform);
                //�����ʒu���Z�b�g
                character.transform.position = characterData.pos;
            }
            Animator animator = null;
            CharacterRendererManager spriteRenderer = null;
            //�e�L�����N�^�[����Animator��SpriteRenderer���擾����
            if (character)
            {
                if (character.TryGetComponent<Animator>(out Animator anim))
                {
                    animator = anim;
                }
                else Debug.LogWarning($"{character.name}��Animator���A�^�b�`���Ă�������");

                if (character.TryGetComponent<CharacterRendererManager>(out CharacterRendererManager crm))
                {
                    spriteRenderer = crm;
                }
                else Debug.Log($"{character.name}�ɂ�CharacterRenererManager���A�^�b�`����Ă��܂���ł���");
            }
            else Debug.LogWarning($"characterList��{characterData.characterName}�ɃI�u�W�F�N�g���A�T�C�����Ă�������");
            //�擾�����R���|�[�l���g�����X�g�Ɋi�[����
            _characterPropaties.Add(Array.IndexOf(_characterList.ToArray(), characterData), new CharacterPropaty(characterName, animator, spriteRenderer));
        }
        //�ŏ��̃e�L�X�g���Ăяo��
        _textUpdateActive = true;
    }
    /// <summary>
    /// �e�L�X�g�{�b�N�X�������ꂽ���Ɏ��s�����C�x���g
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
    /// ���̃e�L�X�g���Ăяo��
    /// </summary>
    public void NextTextTrigger()
    {
        if (_textUpdateActive)
        {
            StartCoroutine(NextText());
        }
    }
    /// <summary>
    /// ���̃e�L�X�g���Ăяo����܂ł̃^�C�}�[���N�����郁�\�b�h
    /// </summary>
    /// <param name="time">�^�C�}�[�̃Z�b�g����</param>
    /// <returns></returns>
    IEnumerator NextTimer(float time)
    {
        _textUpdateActive = false;
        yield return new WaitForSeconds(time);
        _textUpdateActive = true;
    }
    /// <summary>
    /// ���̃e�L�X�g���Ăяo������
    /// </summary>
    /// <returns></returns>
    IEnumerator NextText()
    {
        //�e�L�X�g�X�V���Ƀ{�^���������ꂽ�ꍇ�Ƀe�L�X�g���Ō�܂ŕ\������g���K�[���N������
        if (_nextTextUpdating && _textList[_currentTextNumber].kind == StoryTextList.TextKind.text)
        {
            _textUpdatingCanselTrigger = true;
            yield break;
        }
        //���̃e�L�X�g�ɂ���
        if (_textList.Count - 1 > _currentTextNumber)
        {
            _currentTextNumber++;
        }
        //���̃e�L�X�g���Ȃ��ꍇ�̓z�[���ɋA��
        else
        {
            _mainSystem.BackToHome();
            //�A�Ŗh�~
            if (_nextTimerCoroutine != null) StopCoroutine(_nextTimerCoroutine);
            _textUpdateActive = false;
            yield break;
        }
        _nextTextUpdating = true;
        //���s���Ƃɕ����𕪂���
        string[] texts = _textList[_currentTextNumber].text.Split('\n');
        //textList��kind�ɉ����ē�����ς���
        switch (_textList[_currentTextNumber].kind)
        {
            case StoryTextList.TextKind.move:
                //�����Ă���L�����݂̂��n�C���C�g����
                CharacterHighLight(_textList[_currentTextNumber].characterType);
                //�Ώۂ�Animator��Play���\�b�h�𑗂�
                if (_characterList[_textList[_currentTextNumber].characterType].gameObject)
                {
                    if (_characterPropaties[_textList[_currentTextNumber].characterType].animator != null)
                    {
                        _characterPropaties[_textList[_currentTextNumber].characterType].animator.Play(texts[0]);
                    }
                    else { Debug.LogWarning($"{_characterList[_textList[_currentTextNumber].characterType].gameObject.name}��Animator���A�^�b�`���Ă�������"); }
                }
                else { Debug.LogWarning($"characterList��{_characterList[_textList[_currentTextNumber].characterType].characterName}�ɃI�u�W�F�N�g���A�T�C�����Ă�������"); }
                //�A�j���[�V�����̑ҋ@���Ԃ��v�Z
                float waitTime = 1;
                if (texts.Length > 1)
                {
                    waitTime = float.Parse(texts[1]);
                }
                //�A�j���[�V�������̓e�L�X�g�X�V�𖳌�
                if (_nextTimerCoroutine != null) StopCoroutine(_nextTimerCoroutine);
                _nextTimerCoroutine = StartCoroutine(NextTimer(waitTime));
                yield return new WaitForSeconds(waitTime);
                StartCoroutine(NextText());
                break;

            case StoryTextList.TextKind.text:
                if (_mainUI != null)
                {
                    //�A�Ŗh�~�̂��߂̃^�C�}�[
                    _nextTimerCoroutine = StartCoroutine(NextTimer(0.1f));
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
                        //����CanselTrigger������Γr���ōX�V���~�߂�
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
                //�e�L�X�g�X�V�̏������I���
                _nextTextUpdating = false;
                break;

            case StoryTextList.TextKind.sound:
                //������𐔒l������SoundPlay���\�b�h�𑗂�
                if (int.TryParse(texts[0], out int num) && int.TryParse(texts[1], out int soundNum))
                {
                    _mainSystem.SoundPlay(num, soundNum);
                }
                else Debug.LogWarning("Sound�̎w�肪�K�؂ł͂���܂���ł���");
                break;
        }
    }
    /// <summary>
    /// �����Ă���L�����������n�C���C�g���A����ȊO���Â�����
    /// </summary>
    /// <param name="number">�����Ă���L�����̔ԍ�</param>
    void CharacterHighLight(int number)
    {
        for (int i = 0; i < _characterPropaties.Count; i++)
        {
            if (_characterPropaties[i].characterRendererManager != null)
                _characterPropaties[i].characterRendererManager.ChangeColor(i != number ? _unHighLightColor : Color.white);
        }
    }
}
