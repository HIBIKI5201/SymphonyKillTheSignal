using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HomeUI : MonoBehaviour
{
    UIDocument _homeUI;
    VisualElement _root;

    Button _movementButton;
    Button _collectButton;
    Button _campButton;
    Button _itemButton;

    VisualElement _movementWindow;
    SliderInt _movementSlider;

    private void Start()
    {
        _homeUI = GetComponent<UIDocument>();
        _root = _homeUI.rootVisualElement;

        _movementButton = _root.Q<Button>("MovementButton");
        _movementButton.clicked += OnClickMovementButton;
        _collectButton = _root.Q<Button>("CollectButton");
        _campButton = _root.Q<Button>("CampButton");
        _itemButton = _root.Q<Button>("ItemButton");

        _movementWindow = _root.Q<VisualElement>("MovementWindow");
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
    }

    void OnClickMovementButton()
    {
        _movementWindow.style.display = DisplayStyle.Flex;
    }
}
