using UnityEngine;
using UnityEngine.UIElements;

public class PauseUI : MonoBehaviour
{
    UIDocument _pauseUIDocument;
    VisualElement _root;

    Button _pauseButton;

    void Start()
    {
        _pauseUIDocument = GetComponent<UIDocument>();
        _root = _pauseUIDocument.rootVisualElement;

        _pauseButton = _root.Q<Button>("PauseButton");
        _pauseButton.clicked += PauseManuAppear;
    }

    void PauseManuAppear()
    {
        Debug.Log("pma");
    }

    public void HidePause()
    {
        _pauseUIDocument.enabled = false;
    }

    public void RevealPause()
    {
        _pauseUIDocument.enabled = true;
    }
}
