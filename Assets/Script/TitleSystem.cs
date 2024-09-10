public class TitleSystem : SystemBase
{
    TitleUI _titleUI;

    public override void Initialize()
    {
        _titleUI = GetComponentInChildren<TitleUI>();
        _titleUI.UIAwake(this);
        mainSystem.SoundPlay(MainSystem.AudioPlayKind.BGM, 0);
    }
}
