using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteSounds : MonoBehaviour
{
    [SerializeField] private VolumeControl _volumeControl;

    public void ToggleMasterMute()
    {
        bool isMuted = PlayerPrefs.GetInt("MasterVolume_Enabled", 1) == 0;
        _volumeControl.ToggleMaster(isMuted == false);
    }

    public void SetMute(bool mute)
    {
        _volumeControl.ToggleMaster(mute == false);
    }

    public bool IsMuted()
    {
        return PlayerPrefs.GetInt("MasterVolume_Enabled", 1) == 0;
    }
}
