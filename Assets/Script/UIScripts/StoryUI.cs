using UnityEngine;
using UnityEngine.UIElements;

public class StoryUI : MonoBehaviour
{
    UIDocument _mainUIDocument;
    VisualElement _root;

    Button _button;
    Label _nameLabel;
    Label _textLabel;

    StorySystem _storySystem;

    private void Start()
    {
        _mainUIDocument = GetComponent<UIDocument>();
        _root = _mainUIDocument.rootVisualElement;
        _root.RegisterCallback<KeyDownEvent>(evt =>
        {
            evt.StopImmediatePropagation();
        });

        _storySystem = FindAnyObjectByType<StorySystem>();

        _button = _root.Q<Button>("Button");
        _button.clicked += ButtonClicked;

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
        else { Debug.LogWarning("TextBoxもしくはNameBoxというLabelがみつかりませんでした"); }
    }
}
