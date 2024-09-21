using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class PauseUI : MonoBehaviour
{
    MainSystem _mainSystem;

    UIDocument _pauseUIDocument;
    VisualElement _root;

    Button _pauseButton;

    VisualElement _pauseWindow;
    Button _continueButton;
    Button _saveButton;
    Button _optionButton;
    Button _titleButton;

    Slider _audioMasterSlider;
    Slider _audioBGMSlider;
    Slider _audioSESlider;
    Slider _audioVoiceSlider;
    [SerializeField]
    AudioMixer _audioMixer;
    const string _masterName = "Master";
    float _masterVolume;
    const string _BGMName = "BGM";
    float _BGMVolume;
    const string _SEName = "SoundEffect";
    float _SEVolume;
    const string _VoiceName = "Voice";
    float _VoiceVolume;

    void Start()
    {
        //システムを取得
        _mainSystem = GetComponentInParent<MainSystem>();
        //UIDocumentを取得
        _pauseUIDocument = GetComponent<UIDocument>();
        _root = _pauseUIDocument.rootVisualElement;
        //ポーズボタンを取得
        _pauseButton = _root.Q<Button>("PauseButton");
        _pauseButton.clicked += PauseManuReveal;
        //ウィンドウを取得
        _pauseWindow = _root.Q<VisualElement>("PauseWindow");
        _pauseWindow.style.display = DisplayStyle.None;
        _continueButton = _pauseWindow.Q<Button>("Back");
        _continueButton.clicked += BackButtonClicked;
        _saveButton = _pauseWindow.Q<Button>("Save");
        _saveButton.clicked += SaveButtonClicked;
        _optionButton = _pauseWindow.Q<Button>("Option");
        _optionButton.clicked += OptionButtonClicked;
        _titleButton = _pauseWindow.Q<Button>("Title");
        _titleButton.clicked += TitleButtonClicked;
        //オーディオの初期値
        _audioMixer.GetFloat(_masterName, out _masterVolume);
        _audioMixer.GetFloat(_BGMName, out _BGMVolume);
        _audioMixer.GetFloat(_SEName, out _SEVolume);
        _audioMixer.GetFloat(_VoiceName, out _VoiceVolume);
        _audioMasterSlider = _root.Q<Slider>("MasterSlider");
        _audioMasterSlider.RegisterValueChangedCallback(evt => VolumeSliderChanged(_masterName, _masterVolume, _audioMasterSlider.value));
        VolumeSliderChanged(_masterName, _masterVolume, _audioMasterSlider.value);
        _audioBGMSlider = _root.Q<Slider>("BGMSlider");
        _audioBGMSlider.RegisterValueChangedCallback(evt => VolumeSliderChanged(_BGMName, _BGMVolume, _audioBGMSlider.value));
        VolumeSliderChanged(_BGMName, _BGMVolume, _audioBGMSlider.value);
        _audioSESlider = _root.Q<Slider>("SESlider");
        _audioSESlider.RegisterValueChangedCallback(evt => VolumeSliderChanged(_SEName, _SEVolume, _audioSESlider.value));
        VolumeSliderChanged(_SEName, _SEVolume, _audioSESlider.value);
        _audioVoiceSlider = _root.Q<Slider>("VoiceSlider");
        _audioVoiceSlider.RegisterValueChangedCallback(evt => VolumeSliderChanged(_VoiceName, _VoiceVolume, _audioVoiceSlider.value));
        VolumeSliderChanged(_VoiceName, _VoiceVolume, _audioVoiceSlider.value);
    }

    void VolumeSliderChanged(string name, float originalVolume, float value)
    {
        float db = value / 10 * (originalVolume + 80) - 80;
        Debug.Log($"{name}の音量は{db}デシベル");
        _audioMixer.SetFloat(name, db);
    }

    void PauseManuReveal()
    {
        Debug.Log("pma");
        _pauseWindow.style.display = DisplayStyle.Flex;
        _pauseButton.style.display = DisplayStyle.None;
    }

    public void HidePause()
    {
        _pauseButton.style.display = DisplayStyle.None;
    }

    public void RevealPause()
    {
        _pauseButton.style.display = DisplayStyle.Flex;
    }

    void BackButtonClicked()
    {
        _pauseWindow.style.display = DisplayStyle.None;
        _pauseButton.style.display = DisplayStyle.Flex;
    }

    void SaveButtonClicked()
    {
        _mainSystem.DataSave();
        _pauseWindow.style.display = DisplayStyle.None;
        _pauseButton.style.display = DisplayStyle.Flex;
    }


    void OptionButtonClicked()
    {

    }


    void TitleButtonClicked()
    {
        _pauseWindow.style.display = DisplayStyle.None;
        _mainSystem.BackToTitle();
    }

}
