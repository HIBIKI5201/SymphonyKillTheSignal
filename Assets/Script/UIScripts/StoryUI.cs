using UnityEngine;
using UnityEngine.UIElements;

public class StoryUI : UIBase
{
    Button _button;
    Label _nameLabel;
    Label _textLabel;

    StorySystem _storySystem;

    public override void UIAwake(SystemBase system)
    {
        _storySystem = (StorySystem)system;
        //UI���L�[�ɔ������Ȃ��悤��
        _root.RegisterCallback<KeyDownEvent>(evt =>
        {
            evt.StopImmediatePropagation();
        });
        //�e�L�X�g�{�b�N�X�̃{�^�����擾
        _button = _root.Q<Button>("Button");
        _button.clicked += ButtonClicked;
        //�e�L�X�g�{�b�N�X���擾
        _nameLabel = _root.Q<Label>("NameBox");
        _nameLabel.pickingMode = PickingMode.Ignore;
        _textLabel = _root.Q<Label>("TextBox");
        _textLabel.pickingMode = PickingMode.Ignore;
    }

    void ButtonClicked()
    {
        _storySystem.NextTextTrigger();
    }

    public void TextBoxUpdate(string name, string text)
    {
        if (_textLabel != null && _nameLabel != null)
        {
            _nameLabel.text = name;
            _textLabel.text = text;
        }
        else { Debug.LogWarning("TextBox��������NameBox�Ƃ���Label���݂���܂���ł���"); }
    }
}
