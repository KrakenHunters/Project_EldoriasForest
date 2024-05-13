using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : Menu
{
    [Header("Audio Souce")]
    [SerializeField, Tooltip("Audio Mixer form the Assets folder")]
    private AudioMixer _MasterAudioMixer;

    [Header("Sliders In Settings")]
    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;


    private void Start()
    {
        SetUpVolumeValues();
        ListenForVolumeChange();
    }

    private void SetUpVolumeValues()
    {
        MasterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
    }
    private void ListenForVolumeChange()
    {
        MasterSlider.onValueChanged.AddListener(delegate { SetMasterVolume(); });
        MusicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
        SFXSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
    }

    private void SetMasterVolume()
    {
        _MasterAudioMixer.SetFloat("Master", Mathf.Log10(MasterSlider.value) * 20);
        PlayerPrefs.SetFloat("MasterVolume", MasterSlider.value);
    }
    private void SetMusicVolume()
    {
        _MasterAudioMixer.SetFloat("MusicVolume", Mathf.Log10(MusicSlider.value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);
    }
    private void SetSFXVolume()
    {
        _MasterAudioMixer.SetFloat("SFXVolume", Mathf.Log10(SFXSlider.value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
    }

    public void OnToggleMute()
    {
        isMusted = !isMusted;
        _MasterAudioMixer.SetFloat("Master", isMusted ? Mathf.Log10(MasterSlider.minValue) * 20 : Mathf.Log10(MasterSlider.maxValue) * 20);

    }

}
