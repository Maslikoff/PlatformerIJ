using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private VolumeControl _volumeControl;
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _effectsSlider;
    [SerializeField] private Toggle _muteToggle;

    private void Start()
    {
        SetupSliders();
        LoadSavedValues();
    }

    private void SetupSliders()
    {
        _masterSlider.onValueChanged.AddListener(_volumeControl.ChangeMasterVolume);
        _musicSlider.onValueChanged.AddListener(_volumeControl.ChangeMusicVolume);
        _effectsSlider.onValueChanged.AddListener(_volumeControl.ChangeEffectVolume);
        _muteToggle.onValueChanged.AddListener(_volumeControl.ToggleMaster);
    }

    private void LoadSavedValues()
    {
        _masterSlider.value = PlayerPrefs.GetFloat(AudioConstants.MasterVolume, AudioConstants.DefaultVolume);
        _musicSlider.value = PlayerPrefs.GetFloat(AudioConstants.MusicVolume, AudioConstants.DefaultVolume);
        _effectsSlider.value = PlayerPrefs.GetFloat(AudioConstants.EffectsVolume, AudioConstants.DefaultVolume);
        _muteToggle.isOn = PlayerPrefs.GetInt(AudioConstants.MasterVolumeEnabled, AudioConstants.DefaultEnabled) == 1;
    }
}
