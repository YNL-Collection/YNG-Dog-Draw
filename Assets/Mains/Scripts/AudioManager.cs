using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource backgroundMusicSource;
    public AudioSource soundEffectSource;

    public AudioClip backgroundMusicClip;
    public List<AudioClip> soundEffectsClips;

    private const string SOUND_KEY = "SOUND_ON";
    private const string MUSIC_KEY = "MUSIC_ON";

    private void Start()
    {
        LoadSetting();
        PlayBackgroundMusic();
    }

    private void LoadSetting()
    {
        bool isSoundOn = PlayerPrefs.GetInt(SOUND_KEY, 1) == 1;
        bool isMusicOn = PlayerPrefs.GetInt(MUSIC_KEY, 1) == 1;

        SetSound(isSoundOn);
        SetMusic(isMusicOn);
    }

    public void SetMusic(bool isOn)
    {
        if (isOn)
        {
            backgroundMusicSource.volume = 1f;
            if (!backgroundMusicSource.isPlaying)
                backgroundMusicSource.Play();
        }
        else
        {
            backgroundMusicSource.volume = 0f;
        }
    }

    public void SetSound(bool isOn)
    {
        soundEffectSource.volume = isOn ? 1f : 0f;
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusicClip != null)
        {
            backgroundMusicSource.clip = backgroundMusicClip;
            backgroundMusicSource.loop = true;

            if (PlayerPrefs.GetInt(MUSIC_KEY, 1) == 1)
                backgroundMusicSource.Play();
        }
    }

    public void PlaySoundEffect(int index)
    {
        if (soundEffectSource.volume == 0) return;

        if (IsIndexValid(index, soundEffectsClips.Count))
        {
            soundEffectSource.PlayOneShot(soundEffectsClips[index]);
        }
    }

    public void PlayDrop() => PlaySoundEffect(0);
    public void PlayMerge() => PlaySoundEffect(1);
    public void PlayBtn() => PlaySoundEffect(2);

    private bool IsIndexValid(int index, int arrayLength)
    {
        return index >= 0 && index < arrayLength;
    }
}
