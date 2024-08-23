using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HomeUI : UIBase
{
    HomeSystem _homeSystem;
    AdventureSystem _adventureSystem;
    enum WindowKind
    {
        Movement,
        Collect,
        Camp,
        Item,
    }

    Dictionary<WindowKind, VisualElement> _WindowDictionary;

    Button _movementButton;
    Button _collectButton;
    Button _campButton;
    Button _itemButton;

    VisualElement _currentWindow;

    VisualElement _movementWindow;
    SliderInt _movementSlider;
    int _sliderValue;
    Label _movementDistanceText;
    Label _movemetTimeText;
    Label _movementHealthText;
    Button _movementComformButton;

    VisualElement _campWindow;
    VisualElement _bonfireButton;
    VisualElement _restButton;
    VisualElement _repairButton;
    Dictionary<VisualElement, VisualElement> _campWindowChildrens;
    VisualElement _bonfireWindow;
    VisualElement _restWindow;
    VisualElement _repairWindow;

    public override void UIAwake(SystemBase system)
    {
        //システムを取得
        _homeSystem = (HomeSystem)system;
        _adventureSystem = FindAnyObjectByType<AdventureSystem>();
        //メインボタン系の取得
        _movementButton = _root.Q<Button>("MovementButton");
        _movementButton.RegisterCallback<ClickEvent>(evt => OnclickMainButton(WindowKind.Movement));
        _collectButton = _root.Q<Button>("CollectButton");
        _collectButton.RegisterCallback<ClickEvent>(evt => OnclickMainButton(WindowKind.Collect));
        _campButton = _root.Q<Button>("CampButton");
        _campButton.RegisterCallback<ClickEvent>(evt => OnclickMainButton(WindowKind.Camp));
        _itemButton = _root.Q<Button>("ItemButton");
        _itemButton.RegisterCallback<ClickEvent>(evt => OnclickMainButton(WindowKind.Item));
        //Movement関係の取得と初期化
        _movementWindow = _root.Q<VisualElement>("MovementWindow");
        _movementWindow.style.display = DisplayStyle.None;
        _movementDistanceText = _root.Q<Label>("Movement-DistanceText");
        _movemetTimeText = _root.Q<Label>("Movement-TimeText");
        _movementHealthText = _root.Q<Label>("Movement-HealthText");
        _movementSlider = _root.Q<SliderInt>("Movement-Slider");
        Label label = _movementSlider.Q<Label>();
        label.style.color = Color.white;
        _movementComformButton = _root.Q<Button>("Movement-Button");
        _movementComformButton.clicked += MovementComformButtonClicked;
        //スライダーが変更された時に数値の変更を加える
        if (_movementSlider != null)
        {
            MovementSliderUpdate(_movementSlider.value);
            _movementSlider.RegisterValueChangedCallback(evt =>
            {
                MovementSliderUpdate(_movementSlider.value);
            });
        }
        //Collect関係の取得と初期化

        //Camp関係の取得と初期化
        _campWindow = _root.Q<VisualElement>("CampWindow");
        _campWindow.style.display = DisplayStyle.None;
        _bonfireButton = _root.Q<VisualElement>("Camp-Bonfire");
        _bonfireButton.RegisterCallback<ClickEvent>(evt => CampWindowButtonClicked(_bonfireButton));
        _bonfireWindow = _root.Q<VisualElement>("Camp-BonfireWindow");
        _restButton = _root.Q<VisualElement>("Camp-Rest");
        _restButton.RegisterCallback<ClickEvent>(evt => CampWindowButtonClicked(_restButton));
        _restWindow = _root.Q<VisualElement>("Camp-RestWindow");
        _repairButton = _root.Q<VisualElement>("Camp-Repair");
        _repairButton.RegisterCallback<ClickEvent>(evt => CampWindowButtonClicked(_repairButton));
        _repairWindow = _root.Q<VisualElement>("Camp-RepairWindow");
        _campWindowChildrens = new()
        {
            {_bonfireButton, _bonfireWindow},
            {_restButton, _restWindow},
            {_repairButton, _repairWindow},
        };
        //Item関係の取得と初期化


        //ボタンに対応したウィンドウのエレメントを設定する
        _WindowDictionary = new()
        {
            {WindowKind.Movement, _movementWindow},
            {WindowKind.Collect, null},
            {WindowKind.Camp, _campWindow},
            {WindowKind.Item, null},
        };
    }

    void OnclickMainButton(WindowKind kind)
    {
        if (_currentWindow != null) _currentWindow.style.display = DisplayStyle.None;
        if (_WindowDictionary[kind] != null)
        {
            _WindowDictionary[kind].style.display = DisplayStyle.Flex;
            _currentWindow = _WindowDictionary[kind];
        }
    }

    void MovementSliderUpdate(int value)
    {
        _sliderValue = value;
        _movementDistanceText.text = $"{_adventureSystem.TimeToDistance(value)}km";
        _movemetTimeText.text = $"{value}時間";
        _movementHealthText.text = (value * 5).ToString("0.0");
    }

    void MovementComformButtonClicked()
    {
        Debug.Log("移動開始");
        _homeSystem.Movement(_sliderValue);
        _homeSystem.mainSystem.StoryAction(StoryManager.StoryKind.Movement);
    }

    void CampWindowButtonClicked(VisualElement clickedElement)
    {
        VisualElement[] campWindowButtons = new VisualElement[] {_bonfireButton, _restButton, _repairButton};
        VisualElement[] campWindows = new VisualElement[] {_bonfireWindow, _restWindow, _repairWindow};
        string[] classListNames = new string[] { "camp-button-active", "camp-button-inactive" };
        foreach (VisualElement button in campWindowButtons)
        {
            button.RemoveFromClassList(classListNames[button == clickedElement ? 1 : 0]);
            button.AddToClassList(classListNames[button == clickedElement ? 0 : 1]);
        }
        foreach (VisualElement window in campWindows)
        {
            window.style.display = window == _campWindowChildrens[clickedElement]? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
