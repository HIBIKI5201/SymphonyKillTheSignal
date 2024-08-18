using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeSystem : SystemBase
{
    MainSystem _mainSystem;

    public override void Initialize()
    {
        SceneChanger.CurrentScene = SceneChanger.SceneKind.Home;
        _mainSystem = FindAnyObjectByType<MainSystem>();
    }
}
