using UnityEngine;
using UnityEngine.UI;

public class MuteSounds : MonoBehaviour
{
    [SerializeField] private VolumeControl _volumeControl;
    [SerializeField] private Toggle _toggle;

    private void Start()
    {
        SetupToggle();
        LoadSavedValue();
    }

    private void SetupToggle()
    {
        _toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool isOn)
    {
        _volumeControl.ToggleMaster(isOn);
    }

    private void LoadSavedValue()
    {
        if (_toggle == null)
            return;

        bool isEnabled = PlayerPrefs.GetInt(AudioConstants.MasterVolumeEnabled, AudioConstants.DefaultEnabled) == 1;
        _toggle.isOn = isEnabled;
    }
}