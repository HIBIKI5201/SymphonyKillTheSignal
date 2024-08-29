using AdventureSystems;
public class HomeSystem : SystemBase
{
    HomeUI _homeUI;
    UserDataManager _userDataManager;

    public override void Initialize()
    {
        _userDataManager = mainSystem._userDataManager;
        _homeUI = GetComponentInChildren<HomeUI>();
        _homeUI.UIAwake(this);
        mainSystem.DataSave();
    }

    public void Movement(int value)
    {
        _userDataManager.ChangeDistance(AdventureSystem.MovementTimeToDistance(value));
        _userDataManager.ChangeTime(value);
        _userDataManager.ChangeHealth(-AdventureSystem.MovementTimeToHealth(value));
    }
}