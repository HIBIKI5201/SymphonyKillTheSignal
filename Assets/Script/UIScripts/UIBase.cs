using UnityEngine;
using UnityEngine.UIElements;

public abstract class UIBase : MonoBehaviour
{
    public UIDocument _document;
    public VisualElement _root;

    private void Start()
    {
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;
    }

    public abstract void UIAwake(SystemBase system);
}
