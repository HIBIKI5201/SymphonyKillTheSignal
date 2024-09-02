using AdventureSystems;
using System;
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

    VisualElement _healthBar;
    Label _healthText;
    VisualElement _hungerBar;
    Label _hungerText;
    VisualElement _thirstBar;
    Label _thirstText;

    VisualElement _currentWindow;

    VisualElement _movementWindow;
    SliderInt _movementSlider;
    int _movementSliderValue;
    Label _movementDistanceText;
    Label _movemetTimeText;
    Label _movementHungerText;
    Label _movementHealthText;
    Button _movementComformButton;

    VisualElement _campWindow;
    VisualElement _bonfireButton;
    VisualElement _restButton;
    VisualElement _repairButton;
    Dictionary<VisualElement, VisualElement> _campWindowChildrens;
    VisualElement _bonfireWindow;
    Button _bonfirePlusButton;
    Button _bonfireMinusButton;
    int _bonfireSliderValue;
    Label _bonfireBranchText;
    Label _bonfireRootLevelText;
    Label _bonfireBeLevelText;
    Button _bonfireComformButton;
    VisualElement _restWindow;
    SliderInt _restSlider;
    int _restSliderValue;
    Label _restTimeText;
    Label _restHealthText;
    VisualElement _repairWindow;

    public override void UIAwake(SystemBase system)
    {
        //�V�X�e�����擾
        _homeSystem = (HomeSystem)system;
        //���C���{�^���n�̎擾
        _movementButton = _root.Q<Button>("MovementButton");
        _movementButton.RegisterCallback<ClickEvent>(evt => OnclickMainButton(WindowKind.Movement));
        _collectButton = _root.Q<Button>("CollectButton");
        _collectButton.RegisterCallback<ClickEvent>(evt => OnclickMainButton(WindowKind.Collect));
        _campButton = _root.Q<Button>("CampButton");
        _campButton.RegisterCallback<ClickEvent>(evt => OnclickMainButton(WindowKind.Camp));
        _itemButton = _root.Q<Button>("ItemButton");
        _itemButton.RegisterCallback<ClickEvent>(evt => OnclickMainButton(WindowKind.Item));
        //�p�����[�^�[��ݒ�
        _healthBar = _root.Q<VisualElement>("Health-Bar");
        _healthBar.style.width = new Length(SaveDataManager._mainSaveData.health, LengthUnit.Percent);
        _healthText = _root.Q<Label>("Health-Text");
        _healthText.text = $"{SaveDataManager._mainSaveData.health}/100";
        _hungerBar = _root.Q<VisualElement>("Hunger-Bar");
        _hungerBar.style.width = new Length(SaveDataManager._mainSaveData.hunger, LengthUnit.Percent);
        _hungerText = _root.Q<Label>("Hunger-Text");
        _hungerText.text = $"{SaveDataManager._mainSaveData.hunger}/100";
        _thirstBar = _root.Q<VisualElement>("Thirst-Bar");
        _thirstBar.style.width = new Length(SaveDataManager._mainSaveData.thirst, LengthUnit.Percent);
        _thirstText = _root.Q<Label>("Thirst-Text");
        _thirstText.text = $"{SaveDataManager._mainSaveData.thirst}/100";
        //Movement�֌W�̎擾�Ə�����
        _movementWindow = _root.Q<VisualElement>("MovementWindow");
        _movementWindow.style.display = DisplayStyle.None;
        _movementDistanceText = _root.Q<Label>("Movement-DistanceText");
        _movemetTimeText = _root.Q<Label>("Movement-TimeText");
        _movementHungerText = _root.Q<Label>("Movement-HungerText");
        _movementHealthText = _root.Q<Label>("Movement-HealthText");
        _movementSlider = _root.Q<SliderInt>("Movement-Slider");
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
        //Collect�֌W�̎擾�Ə�����

        //Camp�֌W�̎擾�Ə�����
        _campWindow = _root.Q<VisualElement>("CampWindow");
        _campWindow.style.display = DisplayStyle.None;
        //���΂̃v���p�e�B
        _bonfireButton = _root.Q<VisualElement>("Camp-Bonfire");
        _bonfireButton.RegisterCallback<ClickEvent>(evt => CampWindowButtonClicked(_bonfireButton));
        _bonfireWindow = _root.Q<VisualElement>("Camp-BonfireWindow");
        _bonfireBranchText = _root.Q<Label>("Bonfire-Branch");
        _bonfireRootLevelText = _root.Q<Label>("Bonfire-RootLevel");
        _bonfireRootLevelText.text = $"{SaveDataManager._mainSaveData.campLevel.ToString()} �� ";
        _bonfireBeLevelText = _root.Q<Label>("Bonfire-BeLevel");
        _bonfirePlusButton = _root.Q<Button>("Bonfire-PlusButton");
        _bonfirePlusButton.RegisterCallback<ClickEvent>(evt => BonfireSliderUpdate(1));
        _bonfireMinusButton = _root.Q<Button>("Bonfire-MinusButton");
        _bonfireMinusButton.RegisterCallback<ClickEvent>(evt => BonfireSliderUpdate(-1));
        _bonfireSliderValue = 1;
        BonfireSliderUpdate(0);
        _bonfireComformButton = _root.Q<Button>("Bonfire-Button");
        _bonfireComformButton.clicked += BonfireComformButtonClicked;
        //�x���̃v���p�e�B
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
        //�C���̃v���p�e�B
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
        //Item�֌W�̎擾�Ə�����


        //�{�^���ɑΉ������E�B���h�E�̃G�������g��ݒ肷��
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
        _movemetTimeText.text = $"{value}����";
        _movementHungerText.text = AdventureSystem.MovementTimeToHunger(value).ToString("0.0");
        _movementHealthText.text = AdventureSystem.MovementTimeToHealth(value).ToString("0.0");
    }
    void MovementComformButtonClicked()
    {
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
        if (_bonfireSliderValue + value <= 0 || _bonfireSliderValue + value >= 4)
        {
            return;
        }
        _bonfireSliderValue += value;
        _bonfireBranchText.text = $"{AdventureSystem.BonfireRequireBranch(_bonfireSliderValue)}�{";
        _bonfireBeLevelText.text = Mathf.Min(AdventureSystem.BonfireBecomeLevel(_bonfireSliderValue) + _homeSystem._userDataManager.saveData.campLevel, 8).ToString();
    }
    void BonfireComformButtonClicked()
    {
        _homeSystem.Bonfire(_bonfireSliderValue);
        _homeSystem.mainSystem.StoryAction(StoryManager.StoryKind.Movement);
    }
    void RestSliderUpdate(int value)
    {
        _restSliderValue = value;
        _restTimeText.text = $"{value}����";
        _restHealthText.text = AdventureSystem.RestHealHealth(value).ToString("0.0");
    }

}
