using UnityEngine;
using UnityEngine.UIElements;

public class PauseUI : MonoBehaviour
{
    UIDocument _pauseUIDocument;
    VisualElement _root;

    Button _pauseButton;

    VisualElement _pauseWindow;
    Button _continueButton;
    Button _saveButton;
    Button _optionButton;
    Button _titleButton;

    void Start()
    {
        //UIDocumentを取得
        _pauseUIDocument = GetComponent<UIDocument>();
        _root = _pauseUIDocument.rootVisualElement;
        //ポーズボタンを取得
        _pauseButton = _root.Q<Button>("PauseButton");
        _pauseButton.clicked += PauseManuReveal;
        //ウィンドウを取得
        _pauseWindow = _root.Q<VisualElement>("PauseWindow");
        _pauseWindow.style.display = DisplayStyle.None;
        _continueButton = _pauseWindow.Q<Button>("Back");
        _saveButton = _pauseWindow.Q<Button>("Save");
        _optionButton = _pauseWindow.Q<Button>("Option");
        _titleButton = _pauseWindow.Q<Button>("Title");
    }

    void PauseManuReveal()
    {
        Debug.Log("pma");
        _pauseWindow.style.display = DisplayStyle.Flex;
    }

    public void HidePause()
    {
        _pauseButton.style.display = DisplayStyle.None;
    }

    public void RevealPause()
    {
        _pauseButton.style.display = DisplayStyle.Flex;
    }
}
