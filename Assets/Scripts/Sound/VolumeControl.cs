using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    private const string MasterVolume = "MasterVolume";
    private const string MusicVolume = "MusicVolume";
    private const string EffectsVolume = "EffectsVolume";

    [SerializeField] private AudioMixerGroup _mixer;
    [SerializeField] private float _maxVolume = 0f;
    [SerializeField] private float _minVolume = -80f;
    [SerializeField] private string _masterVolumeButton = MasterVolume + "_Enabled";

    private void Start()
    {
        LoadAllVolumes();
    }

    public void ToggleMusic(bool enable)
    {
        if (enable)
            _mixer.audioMixer.SetFloat(MasterVolume, _maxVolume);
        else
            _mixer.audioMixer.SetFloat(MasterVolume, _minVolume);

        PlayerPrefs.SetInt(_masterVolumeButton, enable ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ChangeMasterVolume(float volume)
    {
        _mixer.audioMixer.SetFloat(MasterVolume, Mathf.Lerp(_minVolume, _maxVolume, volume));

        PlayerPrefs.SetFloat(MasterVolume, volume);
        PlayerPrefs.Save();
    }

    public void ChangeMusicVolume(float volume)
    {
        _mixer.audioMixer.SetFloat(MusicVolume, Mathf.Lerp(_minVolume, _maxVolume, volume));

        PlayerPrefs.SetFloat(MusicVolume, volume);
        PlayerPrefs.Save();
    }

    public void ChangeEffectVolume(float volume)
    {
        _mixer.audioMixer.SetFloat(EffectsVolume, Mathf.Lerp(_minVolume, _maxVolume, volume));

        PlayerPrefs.SetFloat(EffectsVolume, volume);
        PlayerPrefs.Save();
    }

    private void LoadAllVolumes()
    {
        if (PlayerPrefs.HasKey(_masterVolumeButton))
        {
            bool isEnabled = PlayerPrefs.GetInt(_masterVolumeButton, 1) == 1;
            ToggleMusic(isEnabled);
        }

        if (PlayerPrefs.HasKey(MasterVolume))
        {
            float volume = PlayerPrefs.GetFloat(MasterVolume, 1f);
            ChangeMasterVolume(volume);
        }

        if (PlayerPrefs.HasKey(MusicVolume))
        {
            float volume = PlayerPrefs.GetFloat(MusicVolume, 1f);
            ChangeMusicVolume(volume);
        }

        if (PlayerPrefs.HasKey(EffectsVolume))
        {
            float volume = PlayerPrefs.GetFloat(EffectsVolume, 1f);
            ChangeEffectVolume(volume);
        }
    }
}
