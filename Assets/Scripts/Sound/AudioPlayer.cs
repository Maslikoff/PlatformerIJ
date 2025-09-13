using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _mixerGroup;
    [SerializeField] private AudioSource _source;

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        _source.outputAudioMixerGroup = _mixerGroup;
        _source.PlayOneShot(clip, volume);
    }

    public void PlayMusic(AudioClip music, bool loop = true) 
    {
        _source.outputAudioMixerGroup = _mixerGroup;
        _source.clip = music;
        _source.loop = loop;
        _source.Play();
    }
}
