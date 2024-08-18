using UnityEngine;
using UnityEngine.UIElements;

public class TitleUI : MonoBehaviour
{
    TitleSystem _titleSystem;
    UIDocument _titleUI;
    VisualElement _root;

    Button _startButton;
    Button _continueButton;

    public void UIAwake(TitleSystem titleSystem)
    {
        _titleSystem = titleSystem;

        _titleUI = GetComponent<UIDocument>();
        _root = _titleUI.rootVisualElement;

        _startButton = _root.Q<Button>("Start");
        _startButton.clicked += PressStartButton;
        _continueButton = _root.Q<Button>("Continue");
        _continueButton.clicked += PressContinueButton;
    }

    void PressStartButton()
    {
        _titleSystem.MainSystem.GameStart(false);
    }

    void PressContinueButton()
    {
        _titleSystem.MainSystem.GameStart(true);
    }
}