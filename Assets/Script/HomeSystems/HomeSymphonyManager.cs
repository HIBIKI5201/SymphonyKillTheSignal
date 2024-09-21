using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeSymphonyManager : MonoBehaviour
{
    HomeSystem system;
    private void Start()
    {
        system = FindAnyObjectByType<HomeSystem>();
    }

    private void OnMouseDown()
    {
        system.OnSymphonyClicked = true;
    }
}
