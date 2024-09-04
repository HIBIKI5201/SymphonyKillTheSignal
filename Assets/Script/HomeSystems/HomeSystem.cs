using AdventureSystems;
using UnityEngine;
public class HomeSystem : SystemBase
{
    HomeUI _homeUI;
    WorldManager _worldManager;
    public UserDataManager _userDataManager;

    public override void Initialize()
    {
        _userDataManager = mainSystem._userDataManager;
        _homeUI = GetComponentInChildren<HomeUI>();
        _worldManager = GetComponentInChildren<WorldManager>();
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
    }

    public void Bonfire(int value)
    {
        _userDataManager.ChangeTime(1);
        _userDataManager.ChangeHunger(-8);
        _userDataManager.ChangeBonfireLevel(Mathf.Min(AdventureSystem.BonfireBecomeLevel(value) + _userDataManager.saveData.campLevel, 8));
        _userDataManager.ChangeItemValue(UserDataManager.ItemKind.branch, -value * 5);
    }

    public void Rest(int value)
    {
        _userDataManager.ChangeTime(value);
        _userDataManager.ChangeHealth(AdventureSystem.RestHealHealth(value, _userDataManager.saveData.campLevel));
    }
}