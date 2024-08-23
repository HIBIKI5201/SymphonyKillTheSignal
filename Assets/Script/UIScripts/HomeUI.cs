using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HomeUI : UIBase
{
    HomeSystem _homeSystem;
    AdventureSystem _adventureSystem;
    enum WindowKind
    {
        Movement,
        Collect,
        Camp,
        Item,
    }

    struct WindowElement
    {
        public Button button;
        public VisualElement windowElement;

        public WindowElement(Button button, VisualElement visualElement)
        {
            this.button = button;
            this.windowElement = visualElement;
        }
    }

    Dictionary<WindowKind, WindowElement> _WindowDictionary;

    Button _movementButton;
    Button _collectButton;
    Button _campButton;
    Button _itemButton;

    VisualElement _currentWindow;

    VisualElement _movementWindow;
    SliderInt _movementSlider;
    int _sliderValue;
    Label _movenetTimeText;
    Label _movementDistanceText;
    Button _movementComformButton;

    VisualElement _campWindow;
    VisualElement _bonfireButton;
    VisualElement _restButton;
    VisualElement _repairButton;

    public override void UIAwake(SystemBase system)
    {
        //�V�X�e�����擾
        _homeSystem = (HomeSystem)system;
        _adventureSystem = FindAnyObjectByType<AdventureSystem>();
        //���C���{�^���n�̎擾
        _movementButton = _root.Q<Button>("MovementButton");
        _movementButton.RegisterCallback<ClickEvent>(evt => OnclickMainButton(WindowKind.Movement));
        _collectButton = _root.Q<Button>("CollectButton");
        _collectButton.RegisterCallback<ClickEvent>(evt => OnclickMainButton(WindowKind.Collect));
        _campButton = _root.Q<Button>("CampButton");
        _campButton.RegisterCallback<ClickEvent>(evt => OnclickMainButton(WindowKind.Camp));
        _itemButton = _root.Q<Button>("ItemButton");
        _itemButton.RegisterCallback<ClickEvent>(evt => OnclickMainButton(WindowKind.Item));
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
        //Collect�֌W�̎擾�Ə�����


        //Camp�֌W�̎擾�Ə����ݒ�
        _campWindow = _root.Q<VisualElement>("CampWindow");
        _campWindow.style.display = DisplayStyle.None;
        _bonfireButton = _root.Q<VisualElement>("Camp-Bonfire");
        _bonfireButton.RegisterCallback<ClickEvent>(evt => CampWindowButtonClicked(_bonfireButton));
        _restButton = _root.Q<VisualElement>("Camp-Rest");
        _restButton.RegisterCallback<ClickEvent>(evt => CampWindowButtonClicked(_restButton));
        _repairButton = _root.Q<VisualElement>("Camp-Repair");
        _repairButton.RegisterCallback<ClickEvent>(evt => CampWindowButtonClicked(_repairButton));
        //�{�^���ɑΉ������E�B���h�E�̃G�������g��ݒ肷��
        _WindowDictionary = new()
        {
            {WindowKind.Movement, new WindowElement(_movementButton, _movementWindow) },
            {WindowKind.Collect, new WindowElement(null, null)},
            {WindowKind.Camp, new WindowElement(_campButton, _campWindow)},
            {WindowKind.Item, new WindowElement(null, null)},
        };
    }

    void OnclickMainButton(WindowKind kind)
    {
        if (_currentWindow != null) _currentWindow.style.display = DisplayStyle.None;
        if (_WindowDictionary[kind].windowElement != null )
        {
            _WindowDictionary[kind].windowElement.style.display = DisplayStyle.Flex;
            _currentWindow = _WindowDictionary[kind].windowElement;
        }
    }

    void MovementSliderUpdate(int value)
    {
        _sliderValue = value;
        _movenetTimeText.text = value.ToString();
        _movementDistanceText.text = $"{_adventureSystem.TimeToDistance(value)}km";
    }

    void MovementComformButtonClicked()
    {
        Debug.Log("�ړ��J�n");
        _homeSystem.Movement(_sliderValue);
        _homeSystem.mainSystem.StoryAction(StoryManager.StoryKind.Movement);
    }

    void CampWindowButtonClicked(VisualElement clickedElement)
    {
        VisualElement[] campWindowButtons = new VisualElement[] {_bonfireButton, _restButton, _repairButton};
        string[] classListNames = new string[] { "camp-button-active", "camp-button-inactive" };
        foreach (VisualElement button in campWindowButtons)
        {
            if (button == clickedElement)
            {
                button.RemoveFromClassList(classListNames[1]);
                button.AddToClassList(classListNames[0]);
            }
            else
            {
                button.RemoveFromClassList(classListNames[0]);
                button.AddToClassList(classListNames[1]);
            }
        }
    }
}
