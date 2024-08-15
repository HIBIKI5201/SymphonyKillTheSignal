using UnityEngine;
using UnityEngine.UIElements;

public class TitleUI : MonoBehaviour
{
    MainSystem _mainSystem;

    UIDocument _titleUI;
    VisualElement _root;

    Button _startButton;
    Button _continueButton;

    private void Start()
    {
        _mainSystem = FindAnyObjectByType<MainSystem>();

        _titleUI = GetComponent<UIDocument>();
        _root = _titleUI.rootVisualElement;

        _startButton = _root.Q<Button>("Start");
        _startButton.clicked += PressStartButton;
        _continueButton = _root.Q<Button>("Continue");
        _continueButton.clicked += PressContinueButton;
    }

    void PressStartButton()
    {
        _mainSystem.GameStart(false);
    }

    void PressContinueButton()
    {
        _mainSystem.GameStart(true);
    }
}