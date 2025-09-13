using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private float _maxVolume = 0f;
    [SerializeField] private float _minVolume = -80f;

    private void Start()
    {
        LoadAllVolumes();
    }

    public void ToggleMaster(bool enable)
    {
        _mixer.SetFloat(AudioConstants.MasterVolume, enable ? _maxVolume : _minVolume);

        PlayerPrefs.SetInt(AudioConstants.MasterVolume + "_Enabled", enable ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ChangeMasterVolume(float volume)
    {
        ChangeVolume(AudioConstants.MasterVolume, volume);

        PlayerPrefs.SetFloat(AudioConstants.MasterVolume, volume);
        PlayerPrefs.Save();
    }

    public void ChangeMusicVolume(float volume)
    {
        ChangeVolume(AudioConstants.MusicVolume, volume);

        PlayerPrefs.SetFloat(AudioConstants.MusicVolume, volume);
        PlayerPrefs.Save();
    }

    public void ChangeEffectVolume(float volume)
    {
        ChangeVolume(AudioConstants.EffectsVolume, volume);

        PlayerPrefs.SetFloat(AudioConstants.EffectsVolume, volume);
        PlayerPrefs.Save();
    }

    private void ChangeVolume(string parameter, float volume)
    {
        _mixer.SetFloat(parameter, Mathf.Lerp(_minVolume, _maxVolume, volume));
    }

    private void LoadAllVolumes()
    {
        if (PlayerPrefs.HasKey(AudioConstants.MasterVolume + "_Enabled"))
        {
            bool isEnabled = PlayerPrefs.GetInt(AudioConstants.MasterVolume + "_Enabled", 1) == 1;

            ToggleMaster(isEnabled);
        }

        LoadVolume(AudioConstants.MasterVolume);
        LoadVolume(AudioConstants.MusicVolume);
        LoadVolume(AudioConstants.EffectsVolume);
    }

    private void LoadVolume(string volumeKey)
    {
        if (PlayerPrefs.HasKey(volumeKey))
        {
            float volume = PlayerPrefs.GetFloat(volumeKey, 1f);

            ChangeVolume(volumeKey, volume);
        }
    }
}
