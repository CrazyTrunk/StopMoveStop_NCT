
using System;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public SoundData[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    private PlayerData currentPlayerData;

    private void Start()
    {
        currentPlayerData = GameManager.Instance.GetPlayerData();
        PlayMusic(SoundType.THEME);
        ToggleMusicAndSound(currentPlayerData.isSoundOn);
    }
    public void PlayMusic(SoundType type)
    {
        SoundData s = Array.Find(musicSounds, x => x.soundType == type);
        if (s == null)
        {
            Debug.Log("Sound Not Found!");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }
    public void PlaySFX(SoundType type)
    {
        SoundData s = Array.Find(sfxSounds, x => x.soundType == type);
        if (s == null)
        {
            Debug.Log("SFX Not Found!");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }
    public void ToggleMusicAndSound(bool isMusicOn)
    {
        musicSource.mute = !isMusicOn;
        sfxSource.mute = !isMusicOn;
    }
}
