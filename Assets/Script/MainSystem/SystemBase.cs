using UnityEngine;

public abstract class SystemBase : MonoBehaviour
{
    protected MainSystem mainSystem;
    public MainSystem MainSystemPropaty { get => mainSystem; }

    public void SystemAwake(MainSystem mainSystem)
    {
        this.mainSystem = mainSystem;
        Initialize();
    }

    public abstract void Initialize();
}
