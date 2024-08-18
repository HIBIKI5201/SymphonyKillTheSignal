using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeSystem : SystemBase
{
    HomeUI _homeUI;

    public override void Initialize()
    {
        _homeUI = GetComponentInChildren<HomeUI>();
        _homeUI.UIAwake(this);
    }
}
