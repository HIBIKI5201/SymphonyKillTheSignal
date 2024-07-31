using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorySystem : MonoBehaviour
{
    [SerializeField]
    Text textBox;

    [Header("�I�u�W�F�N�g")]
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
        //CharacterNames�����Z�b�g����
        CharacterNames.Clear();
        foreach (var character in characterList)
        {
            CharacterNames.Add(Array.IndexOf(characterList.ToArray(), character), character.characterName);
        }

        //Animators�����Z�b�g����
        Animators.Clear();
        foreach (var character in characterList)
        {
            if (character.gameObject)
            {
                if (character.gameObject.TryGetComponent<Animator>(out Animator animetor))
                {
                    Animators.Add(Array.IndexOf(characterList.ToArray(), character), animetor);
                }
                else { Debug.LogWarning($"{character.gameObject.name}��Animator���A�^�b�`���Ă�������"); }
            }
            else { Debug.LogWarning($"{character}�ɃI�u�W�F�N�g���A�T�C�����Ă�������"); }
        }
    }

    public void NextText()
    {
        // �����Ă���L�����N�^�[�̖��O���擾
        Debug.Log(CharacterNames[textList[textNumber].characterType]);

        //textList��kind�ɉ����ē�����ς���
        switch (textList[textNumber].kind)
        {
            case StoryTextList.TextKind.text:

                //|�������Ɖ��s����
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
                else { Debug.LogWarning("�e�L�X�g�{�b�N�X���A�T�C�����Ă�������"); }

                break;


            case StoryTextList.TextKind.move:

                //�Ώۂ�Animator��Play���\�b�h�𑗂�\��
                Animators[textList[textNumber].characterType].Play(textList[textNumber].text);

                break;


            case StoryTextList.TextKind.effect:

                //������

                break;
        }

        //���̃e�L�X�g�ɂ���
        if (textList.Count - 1 > textNumber)
        {
            textNumber++;
        }
        else { Debug.LogWarning("�e�L�X�g�͏I�����܂���"); }
    }
}
