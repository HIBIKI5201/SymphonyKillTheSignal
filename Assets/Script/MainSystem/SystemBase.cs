using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SystemBase : MonoBehaviour
{
    public MainSystem MainSystem;

    public void SystemAwake(MainSystem mainSystem)
    {
        MainSystem = mainSystem;
        Initialize();
        Debug.Log(MainSystem.gameObject);
    }

    public abstract void Initialize();
}
