using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSystem : SystemBase
{
    TitleUI _titleUI;

    public override void Initialize()
    {
        _titleUI = GetComponentInChildren<TitleUI>();
        _titleUI.UIAwake(this);
    }
}
