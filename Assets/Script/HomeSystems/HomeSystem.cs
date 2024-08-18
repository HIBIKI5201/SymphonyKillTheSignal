using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeSystem : MonoBehaviour
{
    MainSystem _mainSystem;

    private void Start()
    {
        SceneChanger.CurrentScene = SceneChanger.SceneKind.Home;
        _mainSystem = FindAnyObjectByType<MainSystem>();
    }
}
