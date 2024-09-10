using AdventureSystems;
using System;
using UnityEngine;
using static HomeUI;
using static UserDataManager;
public class HomeSystem : SystemBase
{
    HomeUI _homeUI;
    WorldManager _worldManager;
    public UserDataManager _userDataManager;
    public AdventureSystem _adventureSystem;

    public override void Initialize()
    {
        _userDataManager = mainSystem._userDataManager;
        _homeUI = GetComponentInChildren<HomeUI>();
        _adventureSystem = GetComponentInChildren<AdventureSystem>();
        _worldManager = FindAnyObjectByType<WorldManager>();
        _homeUI.UIAwake(this);
        _worldManager.Initialize();
        mainSystem.DataSave();
    }

    public void Movement(int value)
    {
        _userDataManager.ChangeDistance(AdventureSystem.MovementTimeToDistance(value));
        _userDataManager.ChangeTime(value);
        _userDataManager.ChangeHunger(-AdventureSystem.MovementTimeToHunger(value));
        _userDataManager.ChangeHealth(-AdventureSystem.MovementTimeToHealth(value));
        mainSystem.StoryAction(StoryManager.StoryKind.Movement);
    }

    public void Collect(ItemKind collectWindowKind)
    {
        mainSystem.StoryAction(StoryManager.StoryKind.Collect);
        ItemDataBase.ItemCollectData data = _adventureSystem.itemData
            .itemDataList[Array.IndexOf(Enum.GetValues(typeof(ItemKind)), collectWindowKind)].collectData;
        switch (collectWindowKind)
        {
            case ItemKind.branch:
                _userDataManager.ChangeItemValue(ItemKind.branch, UnityEngine.Random.Range(data.getMinValue, data.getMaxValue + 1));
                break;
            case ItemKind.food:
                _userDataManager.ChangeItemValue(ItemKind.food, UnityEngine.Random.Range(data.getMinValue, data.getMaxValue + 1));
                break;
            case ItemKind.dertyWater:
                _userDataManager.ChangeItemValue(ItemKind.dertyWater, UnityEngine.Random.Range(data.getMinValue, data.getMaxValue + 1));
                break;
        }
        _userDataManager.ChangeTime(data.time);
        _userDataManager.ChangeHunger(-data.hunger);
    }

    public void Bonfire(int value)
    {
        _userDataManager.ChangeTime(1);
        _userDataManager.ChangeHunger(-8);
        _userDataManager.ChangeBonfireLevel(Mathf.Min(AdventureSystem.BonfireBecomeLevel(value) + _userDataManager.saveData.campLevel, 8));
        _userDataManager.ChangeItemValue(ItemKind.branch, -value * 5);
        mainSystem.BackToHome();
    }

    public void Rest(int value)
    {
        _userDataManager.ChangeTime(value);
        _userDataManager.ChangeHealth(AdventureSystem.RestHealHealth(value, _userDataManager.saveData.campLevel));
        mainSystem.BackToHome();
    }
}