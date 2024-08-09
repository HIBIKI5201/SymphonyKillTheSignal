using UnityEngine;
using UnityEngine.UIElements;

public class StoryUI : MonoBehaviour
{
    UIDocument _mainUIDocument;
    VisualElement _root;

    Button _button;
    Label _nameLabel;
    Label _textLabel;

    //SliderInt _textBoxSpeedSlider;

    StorySystem _storySystem;

    private void Start()
    {
        _mainUIDocument = GetComponent<UIDocument>();
        _root = _mainUIDocument.rootVisualElement;

        _storySystem = FindAnyObjectByType<StorySystem>();

        _button = _root.Q<Button>("Button");
        _button.clicked += ButtonClicked;

        _nameLabel = _root.Q<Label>("NameBox");
        _textLabel = _root.Q<Label>("TextBox");

        /*
        _textBoxSpeedSlider = _root.Q<SliderInt>("TextBoxSpeedSlider");
        if (_textBoxSpeedSlider != null)
        {
            _storySystem._textSpeed = _textBoxSpeedSlider.value;
            _textBoxSpeedSlider.RegisterValueChangedCallback(evt =>
            {
                _storySystem._textSpeed = evt.newValue;
            });
        }

        */
    }

    void ButtonClicked()
    {
        _storySystem.NextTextTrigger();
    }

    public void TextBoxUpdate(string name, string text)
    {
        if (_textLabel != null && _nameLabel != null)
        {
            _nameLabel.text = name != _storySystem._characterList[0].characterName ? name : "";
            _textLabel.text = text;
        }
        else { Debug.LogWarning("TextBox‚à‚µ‚­‚ÍNameBox‚Æ‚¢‚¤Label‚ª‚Ý‚Â‚©‚è‚Ü‚¹‚ñ‚Å‚µ‚½"); }
    }
}
