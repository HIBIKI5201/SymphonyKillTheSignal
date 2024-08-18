using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HomeUI : MonoBehaviour
{
    MainSystem _mainSystem;

    UIDocument _homeUI;
    VisualElement _root;

    VisualElement _buttonUnactiveElement;

    Dictionary<Button, VisualElement> _buttonToWindow;

    Button _movementButton;
    Button _collectButton;
    Button _campButton;
    Button _itemButton;

    VisualElement _currentWindow;

    VisualElement _movementWindow;
    SliderInt _movementSlider;
    Label _movenetTimeText;
    Label _movementDistanceText;
    Button _movementComformButton;

    private void Start()
    {
        //�V�X�e�����擾
        _mainSystem = FindAnyObjectByType<MainSystem>();
        //UIDocument�̎擾
        _homeUI = GetComponent<UIDocument>();
        _root = _homeUI.rootVisualElement;
        //ButtonUnactiveElement�̎擾
        _buttonUnactiveElement = _root.Q<VisualElement>("ButtonUnactiveElement");
        _buttonUnactiveElement.style.display = DisplayStyle.None;
        //���C���{�^���n�̎擾
        _movementButton = _root.Q<Button>("MovementButton");
        _movementButton.clicked += OnClickMovementButton;
        _collectButton = _root.Q<Button>("CollectButton");
        _collectButton.clicked += OnClickCollectButton;
        _campButton = _root.Q<Button>("CampButton");
        _campButton.clicked += OnClickCampButton;
        _itemButton = _root.Q<Button>("ItemButton");
        _itemButton.clicked += OnClickItemButton;
        //Movement�֌W�̎擾�Ə����ݒ�
        _movementWindow = _root.Q<VisualElement>("MovementWindow");
        _movementWindow.style.display = DisplayStyle.None;
        _movenetTimeText = _root.Q<Label>("Movement-TimeText");
        _movementDistanceText = _root.Q<Label>("Movement-DistanceText");
        _movementSlider = _root.Q<SliderInt>("Movement-Slider");
        Label label = _movementSlider.Q<Label>();
        label.style.color = Color.white;
        _movementComformButton = _root.Q<Button>("Movement-Button");
        _movementComformButton.clicked += MovementComformButtonClicked;
        //�X���C�_�[���ύX���ꂽ���ɐ��l�̕ύX��������
        if (_movementSlider != null)
        {
            MovementSliderUpdate(_movementSlider.value);
            _movementSlider.RegisterValueChangedCallback(evt =>
            {
                MovementSliderUpdate(_movementSlider.value);
            });
        }
        //�{�^���ɑΉ������E�B���h�E�̃G�������g��ݒ肷��
        _buttonToWindow = new()
        {
            {_movementButton, _movementWindow},
            {_campButton, null},
            {_collectButton, null},
            {_itemButton, null},
        };
    }
    void WindowHide()
    {
        _currentWindow.style.display = DisplayStyle.None;
    }

    void OnclickButton(Button button)
    {
        if (_currentWindow != null) WindowHide();
        _buttonToWindow[button].style.display = DisplayStyle.Flex;
        _currentWindow = _buttonToWindow[button];
    }

    void OnClickMovementButton()
    {
        OnclickButton(_movementButton);
    }

    void OnClickCollectButton()
    {

    }

    void OnClickCampButton()
    {

    }

    void OnClickItemButton()
    {

    }

    void MovementSliderUpdate(int value)
    {
        _movenetTimeText.text = value.ToString();
        _movementDistanceText.text = $"{(value * 3).ToString()}km";
    }

    void MovementComformButtonClicked()
    {
        _buttonUnactiveElement.style.display = DisplayStyle.Flex;
        Debug.Log("�ړ��J�n");
        _mainSystem.BackToTitle();
    }
}
