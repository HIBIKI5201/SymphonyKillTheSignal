using TMPro.EditorUtilities;

public class HomeSystem : SystemBase
{
    HomeUI _homeUI;
    UserDataManager _userDataManager;

    AdventureSystem _adventureSystem;

    public override void Initialize()
    {
        _userDataManager = mainSystem._userDataManager;
        _adventureSystem = FindAnyObjectByType<AdventureSystem>();
        _homeUI = GetComponentInChildren<HomeUI>();
        _homeUI.UIAwake(this);
        SaveDataManager.Save(_userDataManager.saveData);
    }

    public void Movement(int value)
    {
        _userDataManager.ChangeDistance(_adventureSystem.TimeToDistance(value));
        _userDataManager.ChangeTime(value);
    }
}