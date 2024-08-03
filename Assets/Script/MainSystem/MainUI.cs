using UnityEngine;
using UnityEngine.UIElements;

public class MainUI : MonoBehaviour
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

        _button = _root.Q<Button>("Button");
        _button.clicked += ButtonClicked;

        _nameLabel = _root.Q<Label>("NameBox");
        _textLabel = _root.Q<Label>("TextBox");

        _storySystem = FindAnyObjectByType<StorySystem>();
    }

    void ButtonClicked()
    {
        if (!_storySystem._nextTextUpdating)
        {
            StartCoroutine(_storySystem.WaitNextText());
        }
    }

    public void TextBoxUpdate(string name, string text)
    {
        if (_textLabel != null && _nameLabel != null)
        {
            _nameLabel.text = name;
            _textLabel.text = text;
        }
        else { Debug.LogWarning("TextBox‚à‚µ‚­‚ÍNameBox‚Æ‚¢‚¤Label‚ª‚Ý‚Â‚©‚è‚Ü‚¹‚ñ‚Å‚µ‚½"); }
    }
}
