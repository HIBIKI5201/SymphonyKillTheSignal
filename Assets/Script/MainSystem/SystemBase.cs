using UnityEngine;

public abstract class SystemBase : MonoBehaviour
{
    public MainSystem MainSystem;

    public void SystemAwake(MainSystem mainSystem)
    {
        MainSystem = mainSystem;
        Initialize();
    }

    public abstract void Initialize();
}
