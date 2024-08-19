public class HomeSystem : SystemBase
{
    HomeUI _homeUI;

    public override void Initialize()
    {
        _homeUI = GetComponentInChildren<HomeUI>();
        _homeUI.UIAwake(this);
    }
}