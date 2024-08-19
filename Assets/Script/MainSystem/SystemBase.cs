using UnityEngine;

public abstract class SystemBase : MonoBehaviour
{
    public MainSystem mainSystem;

    public void SystemAwake(MainSystem mainSystem)
    {
        this.mainSystem = mainSystem;
        Initialize();
    }

    public abstract void Initialize();
}
