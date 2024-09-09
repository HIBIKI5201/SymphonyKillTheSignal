using AdventureSystems;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.InputManagerEntry;
using static UserDataManager;
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

    VisualElement _collectWindow;
    VisualElement _collectBranchButton;
    VisualElement _collectFoodButton;
    VisualElement _collectWaterButton;

    ItemKind _collectWindowKind;
    Dictionary<VisualElement, ItemKind> _collectWindowDictionary;
    Label _collectGetItemListText;
    Label _collectTimeText;
    Label _collectHungerText;
    Button _collectComformButton;

    VisualElement _campWindow;
    VisualElement _bonfireButton;
    VisualElement _restButton;
    VisualElement _craftButton;
    Dictionary<VisualElement, VisualElement> _campWindowChildrens;
    VisualElement _bonfireWindow;
    Button _bonfirePlusButton;
    Button _bonfireMinusButton;
    int _bonfireSliderValue;
    Label _bonfireBranchText;
    Label _bonfireRootLevelText;
    Label _bonfireBeLevelText;
    VisualElement _bonfireImage;
    [SerializeField] List<Sprite> _bonfireImagesList;
    Button _bonfireComformButton;
    VisualElement _restWindow;
    SliderInt _restSlider;
    int _restSliderValue;
    Label _restTimeText;
    Label _restHealthText;
    Button _restComformButton;
    VisualElement _craftWindow;

    VisualElement _inventoryWindow;
    List<(ItemKind kind, VisualElement icon)> inventoryItemsList = new();

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
        //パラメーターを設定
        _healthBar = _root.Q<VisualElement>("Health-Bar");
        _healthBar.style.width = new Length(SaveDataManager._mainSaveData.health, LengthUnit.Percent);
        _healthText = _root.Q<Label>("Health-Text");
        _healthText.text = $"{SaveDataManager._mainSaveData.health} / 100";
        _hungerBar = _root.Q<VisualElement>("Hunger-Bar");
        _hungerBar.style.width = new Length(SaveDataManager._mainSaveData.hunger, LengthUnit.Percent);
        _hungerText = _root.Q<Label>("Hunger-Text");
        _hungerText.text = $"{SaveDataManager._mainSaveData.hunger} / 100";
        _thirstBar = _root.Q<VisualElement>("Thirst-Bar");
        _thirstBar.style.width = new Length(SaveDataManager._mainSaveData.thirst, LengthUnit.Percent);
        _thirstText = _root.Q<Label>("Thirst-Text");
        _thirstText.text = $"{SaveDataManager._mainSaveData.thirst} / 100";
        //Movement関係の取得と初期化
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
        //Collect関係の取得と初期化
        _collectWindow = _root.Q<VisualElement>("CollectWindow");
        _collectWindow.style.display = DisplayStyle.None;
        _collectComformButton = _root.Q<Button>("Collect-Button");
        _collectComformButton.clicked += CollectComformButtonClicked;
        _collectBranchButton = _root.Q<VisualElement>("Collect-Branch");
        _collectBranchButton.RegisterCallback<ClickEvent>(evt => CollectWindowButtonClicked(_collectBranchButton));
        _collectFoodButton = _root.Q<VisualElement>("Collect-Food");
        _collectFoodButton.RegisterCallback<ClickEvent>(evt => CollectWindowButtonClicked(_collectFoodButton));
        _collectWaterButton = _root.Q<VisualElement>("Collect-Water");
        _collectWaterButton.RegisterCallback<ClickEvent>(evt => CollectWindowButtonClicked(_collectWaterButton));
        _collectGetItemListText = _root.Q<Label>("Collect-GetItemList");
        _collectTimeText = _root.Q<Label>("Collect-Time");
        _collectHungerText = _root.Q<Label>("Collect-Hunger");
        _collectWindowDictionary = new()
        {
            {_collectBranchButton, ItemKind.branch },
            {_collectFoodButton, ItemKind.food },
            {_collectWaterButton, ItemKind.dertyWater },
        };
        CollectWindowButtonClicked(_collectBranchButton);
        //Camp関係の取得と初期化
        _campWindow = _root.Q<VisualElement>("CampWindow");
        _campWindow.style.display = DisplayStyle.None;
        //焚火のプロパティ
        _bonfireButton = _root.Q<VisualElement>("Camp-Bonfire");
        _bonfireButton.RegisterCallback<ClickEvent>(evt => CampWindowButtonClicked(_bonfireButton));
        _bonfireWindow = _root.Q<VisualElement>("Camp-BonfireWindow");
        _bonfireBranchText = _root.Q<Label>("Bonfire-Branch");
        _bonfireRootLevelText = _root.Q<Label>("Bonfire-RootLevel");
        _bonfireRootLevelText.text = $"{SaveDataManager._mainSaveData.campLevel} → ";
        _bonfireBeLevelText = _root.Q<Label>("Bonfire-BeLevel");
        _bonfireImage = _root.Q<VisualElement>("Bonfire-Image");
        _bonfirePlusButton = _root.Q<Button>("Bonfire-PlusButton");
        _bonfirePlusButton.RegisterCallback<ClickEvent>(evt => BonfireSliderUpdate(1));
        _bonfireMinusButton = _root.Q<Button>("Bonfire-MinusButton");
        _bonfireMinusButton.RegisterCallback<ClickEvent>(evt => BonfireSliderUpdate(-1));
        _bonfireSliderValue = 1;
        BonfireSliderUpdate(0);
        _bonfireComformButton = _root.Q<Button>("Bonfire-Button");
        _bonfireComformButton.clicked += BonfireComformButtonClicked;
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
        _restComformButton = _root.Q<Button>("Rest-Button");
        _restComformButton.clicked += RestComformButtonClicked;
        //修理のプロパティ
        _craftButton = _root.Q<VisualElement>("Camp-Craft");
        _craftButton.RegisterCallback<ClickEvent>(evt => CampWindowButtonClicked(_craftButton));
        _craftWindow = _root.Q<VisualElement>("Camp-CraftWindow");
        _campWindowChildrens = new()
        {
            {_bonfireButton, _bonfireWindow},
            {_restButton, _restWindow},
            {_craftButton, _craftWindow},
        };
        CampWindowButtonClicked(_bonfireButton);
        //Inventory関係の取得と初期化
        _inventoryWindow = _root.Q<VisualElement>("InventoryWindow");
        _inventoryWindow.style.display = DisplayStyle.None;
        inventoryItemsList.Add((ItemKind.branch, _root.Q<VisualElement>("Inventory-Branch")));
        inventoryItemsList.Add((ItemKind.food, _root.Q<VisualElement>("Inventory-Berry")));
        inventoryItemsList.Add((ItemKind.dertyWater, _root.Q<VisualElement>("Inventory-Water")));
        inventoryItemsList.Add((ItemKind.dertyWater, _root.Q<VisualElement>("Inventory-DertyWater")));
        foreach (var item in inventoryItemsList)
        {
            Debug.Log(_homeSystem._userDataManager.saveData.itemList);
            int value = _homeSystem._userDataManager.saveData.itemList[Array.IndexOf(Enum.GetValues(typeof(ItemKind)), item.kind)];
            if (value > 0)
            {
            item.icon.Q<Label>("Inventory-ItemValue").text = $"×{value}";
            }
            else
            {
                item.icon.style.display = DisplayStyle.None;
            }
        }
        //ボタンに対応したウィンドウのエレメントを設定する
        _WindowDictionary = new()
        {
            {WindowKind.Movement, _movementWindow},
            {WindowKind.Collect, _collectWindow},
            {WindowKind.Camp, _campWindow},
            {WindowKind.Item, _inventoryWindow},
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
        _movementHungerText.text = AdventureSystem.MovementTimeToHunger(value).ToString("0.0");
        _movementHealthText.text = AdventureSystem.MovementTimeToHealth(value).ToString("0.0");
    }
    void MovementComformButtonClicked()
    {
        _homeSystem.Movement(_movementSliderValue);
    }
    void CollectWindowButtonClicked(VisualElement clickedElement)
    {
        VisualElement[] collectWindowButtons = new VisualElement[] { _collectBranchButton, _collectFoodButton, _collectWaterButton };
        string[] classListNames = new string[] { "collect-button-active", "collect-button-inactive" };
        foreach (VisualElement button in collectWindowButtons)
        {
            button.RemoveFromClassList(classListNames[button == clickedElement ? 1 : 0]);
            button.AddToClassList(classListNames[button == clickedElement ? 0 : 1]);
        }
        _collectWindowKind = _collectWindowDictionary[clickedElement];
        int index = Array.IndexOf(Enum.GetValues(typeof(ItemKind)), _collectWindowKind);
        ItemDataBase.ItemData itemData = _homeSystem._adventureSystem.itemData.itemDataList[index];
        _collectGetItemListText.text = itemData.itemKind;
        _collectTimeText.text = $"{itemData.time}時間";
        _collectHungerText.text = $"{itemData.hunger}";
    }

    void CollectComformButtonClicked()
    {
        _homeSystem.Collect(_collectWindowKind);
    }
    void CampWindowButtonClicked(VisualElement clickedElement)
    {
        VisualElement[] campWindowButtons = new VisualElement[] { _bonfireButton, _restButton, _craftButton };
        VisualElement[] campWindows = new VisualElement[] { _bonfireWindow, _restWindow, _craftWindow };
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
        _bonfireBranchText.text = $"{AdventureSystem.BonfireRequireBranch(_bonfireSliderValue)}本";
        _bonfireBeLevelText.text = Mathf.Min(AdventureSystem.BonfireBecomeLevel(_bonfireSliderValue) + _homeSystem._userDataManager.saveData.campLevel, 8).ToString();
        _bonfireImage.style.backgroundImage = _bonfireImagesList[Mathf.Min(AdventureSystem.BonfireBecomeLevel(_bonfireSliderValue) + _homeSystem._userDataManager.saveData.campLevel, 8) / 2].texture;
    }
    void BonfireComformButtonClicked()
    {
        _homeSystem.Bonfire(_bonfireSliderValue);
    }
    void RestSliderUpdate(int value)
    {
        _restSliderValue = value;
        _restTimeText.text = $"{value}時間";
        _restHealthText.text = AdventureSystem.RestHealHealth(value, _homeSystem._userDataManager.saveData.campLevel).ToString("0.0");
    }
    void RestComformButtonClicked()
    {
        _homeSystem.Rest(_restSliderValue);
    }
}
