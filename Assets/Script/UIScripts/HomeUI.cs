using AdventureSystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class HomeUI : UIBase
{
    HomeSystem _homeSystem;
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
    int _movementSliderValue;
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
    SliderInt _bonfireSlider;
    int _bonfireSliderValue;
    Label _bonfireBranchText;
    Label _bonfireTimeText;
    Label _bonfireBeLevelText;
    VisualElement _restWindow;
    SliderInt _restSlider;
    int _restSliderValue;
    Label _restTimeText;
    Label _restHealthText;
    VisualElement _repairWindow;

    public override void UIAwake(SystemBase system)
    {
        //システムを取得
        _homeSystem = (HomeSystem)system;
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
        //焚火のプロパティ
        _bonfireButton = _root.Q<VisualElement>("Camp-Bonfire");
        _bonfireButton.RegisterCallback<ClickEvent>(evt => CampWindowButtonClicked(_bonfireButton));
        _bonfireWindow = _root.Q<VisualElement>("Camp-BonfireWindow");
        _bonfireBranchText = _root.Q<Label>("Bonfire-Branch");
        _bonfireTimeText = _root.Q<Label>("Bonfire-Time");
        _bonfireBeLevelText = _root.Q<Label>("Bonfire-BeLevel");
        _bonfireSlider = _root.Q<SliderInt>("Bonfire-Slider");
        if (_bonfireSlider != null)
        {
            BonfireSliderUpdate(_bonfireSlider.value);
            _bonfireSlider.RegisterValueChangedCallback(evt =>
            {
                BonfireSliderUpdate(_bonfireSlider.value);
            });
        }
        //休息のプロパティ
        _restButton = _root.Q<VisualElement>("Camp-Rest");
        _restButton.RegisterCallback<ClickEvent>(evt => CampWindowButtonClicked(_restButton));
        _restWindow = _root.Q<VisualElement>("Camp-RestWindow");
        _restTimeText = _root.Q<Label>("Rest-Time");
        _restHealthText = _root.Q<Label>("Rest-Health");
        _restSlider = _root.Q<SliderInt>("Rest-Slider");
        if (_restSlider != null)
        {
            RestSliderUpdate(_restSlider.value);
            _restSlider.RegisterValueChangedCallback(evt =>
            {
                RestSliderUpdate(_restSlider.value);
            });
        }
        //修理のプロパティ
        _repairButton = _root.Q<VisualElement>("Camp-Repair");
        _repairButton.RegisterCallback<ClickEvent>(evt => CampWindowButtonClicked(_repairButton));
        _repairWindow = _root.Q<VisualElement>("Camp-RepairWindow");
        _campWindowChildrens = new()
        {
            {_bonfireButton, _bonfireWindow},
            {_restButton, _restWindow},
            {_repairButton, _repairWindow},
        };
        CampWindowButtonClicked(_bonfireButton);
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
        _movementSliderValue = value;
        _movementDistanceText.text = $"{AdventureSystem.MovementTimeToDistance(value)}km";
        _movemetTimeText.text = $"{value}時間";
        _movementHealthText.text = (AdventureSystem.MovementTimeToHealth(value)).ToString("0.0");
    }
    void MovementComformButtonClicked()
    {
        Debug.Log("移動開始");
        _homeSystem.Movement(_movementSliderValue);
        _homeSystem.mainSystem.StoryAction(StoryManager.StoryKind.Movement);
    }
    void CampWindowButtonClicked(VisualElement clickedElement)
    {
        VisualElement[] campWindowButtons = new VisualElement[] { _bonfireButton, _restButton, _repairButton };
        VisualElement[] campWindows = new VisualElement[] { _bonfireWindow, _restWindow, _repairWindow };
        string[] classListNames = new string[] { "camp-button-active", "camp-button-inactive" };
        foreach (VisualElement button in campWindowButtons)
        {
            button.RemoveFromClassList(classListNames[button == clickedElement ? 1 : 0]);
            button.AddToClassList(classListNames[button == clickedElement ? 0 : 1]);
        }
        foreach (VisualElement window in campWindows)
        {
            window.style.display = window == _campWindowChildrens[clickedElement] ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
    void BonfireSliderUpdate(int value)
    {
        _bonfireSliderValue = value;
        _bonfireTimeText.text = $"{value}時間";
        _bonfireBranchText.text = $"{AdventureSystem.BonfireRequireBranch(value)}";
        _bonfireBeLevelText.text = AdventureSystem.BonfireBecomeLevel(value).ToString();
    }
    void RestSliderUpdate(int value)
    {
        _restSliderValue = value;
        _restTimeText.text = $"{value}時間";
        _restHealthText.text = AdventureSystem.RestHealHealth(value).ToString("0.0");
    }

}
