using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSystemLoader : MonoBehaviour
{
    [SerializeField]
    string MainSystemSceneName = "SystemScene";

    private void Start()
    {
        SceneManager.LoadScene(MainSystemSceneName, LoadSceneMode.Additive);
        Destroy(gameObject);
    }
}
