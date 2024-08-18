using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class ScreenEffectUI : MonoBehaviour
{
    UIDocument _screenEffectUIDocument;
    VisualElement _root;

    VisualElement _fade;
    VisualElement _buttonUnactiveElement;
    void Start()
    {
        _screenEffectUIDocument = GetComponent<UIDocument>();
        _root = _screenEffectUIDocument.rootVisualElement;
        //フェードの取得
        _fade = _root.Q<VisualElement>("Fade");
        _fade.pickingMode = PickingMode.Ignore;
        //ButtonUnactiveElementの取得
        _buttonUnactiveElement = _root.Q<VisualElement>("ButtonUnactiveElement");
        _buttonUnactiveElement.style.display = DisplayStyle.None;
    }

    public void ScreenFadeOut(float timer)
    {
        DOTween.To(() => new Color(0, 0, 0, 0), x => _fade.style.backgroundColor = x, new Color(0, 0, 0, 1), timer).SetEase(Ease.Linear);
    }

    public void ScreenFadeIn(float timer)
    {
        DOTween.To(() => new Color(0, 0, 0, 1), x => _fade.style.backgroundColor = x, new Color(0, 0, 0, 0), timer).SetEase(Ease.Linear);
    }

    public void ButtonUnactiveElement(bool active)
    {
        if (active)
        {
            _buttonUnactiveElement.style.display = DisplayStyle.Flex;
        }
        else
        {
            _buttonUnactiveElement.style.display = DisplayStyle.None;
        }
    }
}
