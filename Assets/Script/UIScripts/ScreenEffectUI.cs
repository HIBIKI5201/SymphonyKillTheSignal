using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class ScreenEffectUI : MonoBehaviour
{
    UIDocument _screenEffectUIDocument;
    VisualElement _root;

    VisualElement _fade;
    void Start()
    {
        _screenEffectUIDocument = GetComponent<UIDocument>();
        _root = _screenEffectUIDocument.rootVisualElement;

        _fade = _root.Q<VisualElement>("Fade");
        _fade.pickingMode = PickingMode.Ignore;
    }

    void Update()
    {

    }

    public void ScreenEffect()
    {
        DOTween.To(() => new Color(0, 0, 0, 1), x => _fade.style.backgroundColor = x, new Color(0, 0, 0, 0), 3).SetEase(Ease.Linear);
    }
}
