using UnityEngine.UIElements;

public class TitleUI : UIBase
{
    TitleSystem _titleSystem;

    Button _startButton;
    Button _continueButton;

    public override void UIAwake(SystemBase system)
    {
        _titleSystem = (TitleSystem)system;
        //メインタイトルのボタンを取得
        _startButton = _root.Q<Button>("Start");
        _startButton.clicked += PressStartButton;
        _continueButton = _root.Q<Button>("Continue");
        _continueButton.clicked += PressContinueButton;
    }

    void PressStartButton()
    {
        _titleSystem.MainSystemPropaty.GameStart(false);
    }

    void PressContinueButton()
    {
        _titleSystem.MainSystemPropaty.GameStart(true);
    }
}