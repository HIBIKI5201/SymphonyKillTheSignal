using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HomeUI : MonoBehaviour
{
    UIDocument _homeUI;
    VisualElement _root;

    Dictionary<Button, VisualElement> _buttonToWindow;

    Button _movementButton;
    Button _collectButton;
    Button _campButton;
    Button _itemButton;

    VisualElement _currentWindow;

    VisualElement _movementWindow;
    SliderInt _movementSlider;

    private void Start()
    {
        //UIDocumentの取得
        _homeUI = GetComponent<UIDocument>();
        _root = _homeUI.rootVisualElement;
        //メインボタン系の取得
        _movementButton = _root.Q<Button>("MovementButton");
        _movementButton.clicked += OnClickMovementButton;
        _collectButton = _root.Q<Button>("CollectButton");
        _collectButton.clicked += OnClickCollectButton;
        _campButton = _root.Q<Button>("CampButton");
        _campButton.clicked += OnClickCampButton;
        _itemButton = _root.Q<Button>("ItemButton");
        _itemButton.clicked += OnClickItemButton;
        //Movement関係の取得と初期設定
        _movementWindow = _root.Q<VisualElement>("MovementWindow");
        _movementWindow.style.display = DisplayStyle.None;
        _movementSlider = _root.Q<SliderInt>("TextBoxSpeedSlider");
        /*
        if (_movementSlider != null)
        {
            _storySystem._textSpeed = _textBoxSpeedSlider.value;
            _textBoxSpeedSlider.RegisterValueChangedCallback(evt =>
            {
                _storySystem._textSpeed = evt.newValue;
            });
        }
        */

        //ボタンに対応したウィンドウのエレメントを設定する
        _buttonToWindow = new()
        {
            {_movementButton, _movementWindow},
            {_campButton, _campButton},
            {_collectButton, _collectButton},
            {_itemButton, _itemButton},
        };
    }
    void WindowHide()
    {
        _currentWindow.style.display = DisplayStyle.None;
    }

    void OnclickButton(Button button)
    {
        if (_currentWindow != null) WindowHide();
        _buttonToWindow[button].style.display = DisplayStyle.Flex;
        _currentWindow = _buttonToWindow[button];
    }

    void OnClickMovementButton()
    {
        OnclickButton(_movementButton);
    }

    void OnClickCollectButton()
    {

    }

    void OnClickCampButton()
    {

    }

    void OnClickItemButton()
    {

    }
}
