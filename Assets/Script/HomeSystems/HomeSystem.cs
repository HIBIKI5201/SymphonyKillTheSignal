public class HomeSystem : SystemBase
{
    HomeUI _homeUI;
    UserDataManager _userDataManager;

    public override void Initialize()
    {
        _userDataManager = mainSystem._userDataManager;

        _homeUI = GetComponentInChildren<HomeUI>();
        _homeUI.UIAwake(this);
        SaveDataManager.Save(_userDataManager.saveData);
    }

    public void Movement(int time, int distance)
    {
        _userDataManager.ChangeDistance(time);
        _userDataManager.ChangeTime(distance);
    }
}