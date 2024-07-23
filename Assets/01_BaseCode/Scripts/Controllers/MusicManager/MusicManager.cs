﻿using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

public enum NameMusic
{
    None = 0,
    Fly = 1,
    FlyOut = 2,
    Idle = 3,
    Beep = 4
}

public class MusicData
{
    public NameMusic name;
    public float spaceDelayTime;
    [HideInInspector] public float timmer;
    [HideInInspector] public bool isActiving;
    public AudioClip audioClip;
}

public class MusicManager : SerializedMonoBehaviour
{
    public Dictionary<NameMusic, MusicData> clips;

    public enum SourceAudio
    {
        Music,
        Effect,
        UI
    };

    public AudioMixer mixer;
    public AudioSource effectSource;
    public AudioSource musicSource;
    public AudioSource soundUISource;
    public float lowPitchRange = 0.95f; //The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.05f; //The highest a sound effect will be randomly pitched.
    public float delayTime = 0.5f;
    public const string MASTER_KEY = "MASTER_KEY";
    public const string MUSIC_KEY = "MUSIC_KEY";
    public const string SOUND_KEY = "SOUND_KEY";
    public const float MIN_VALUE = -80f;
    public const float MAX_VALUE = 0f;

    [SerializeField] private AudioClip winMusic;
    [SerializeField] private AudioClip HomeSceneBGMusic;
    [SerializeField] private AudioClip LoadingSceneBGMusic;
    [SerializeField] private AudioClip GameplayBGMusic;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip clickSound2;
    [SerializeField] private AudioClip clickSound3;
    [SerializeField] private AudioClip pigBankCoinAppear;
    [SerializeField] private AudioClip coinEffectMove;
    [SerializeField] private AudioClip uiSoftToggle;
    [SerializeField] private AudioClip pieceDone;

    [Header("Story Mode")]
    [SerializeField] private AudioClip FunnyGamePlayMusic;
    [SerializeField] private AudioClip SadGamePlayMusic;
    [SerializeField] private AudioClip NoelPlayMusic;
    [SerializeField] private AudioClip CorrectDragMusic;
    [SerializeField] private AudioClip WrongtDragMusic;

    [SerializeField] private AudioClip DonePieceMusic;
    [SerializeField] private AudioClip DonStoryMusic;


    private AudioClip _currentMusic;

    #region MusicManager Methods

    public float MasterVolume
    {
        get { return PlayerPrefs.GetFloat(MASTER_KEY, 0f); }
        set { SetMasterVolume(value); }
    }

    public float MusicVolume
    {
        get { return UseProfile.OnMusic ? 1 : 0; }
    }

    public float SoundVolume
    {
        get { return UseProfile.OnSound ? 1 : 0; }
    }

    public void Init()
    {
        musicSource.volume = UseProfile.OnMusic ? 0.7f : 0;
        effectSource.volume = UseProfile.OnSound ? 1 : 0;
    }

    //Used to play single sound clips.
    public void PlaySingle(AudioClip clip, SourceAudio source = SourceAudio.Effect)
    {
        if (clip == null)
            return;
        switch (source)
        {
            case SourceAudio.Music:
                if (MusicVolume == 0) return;
                musicSource.clip = clip;
                musicSource.Play();
                break;
            case SourceAudio.Effect:
                if (SoundVolume == 0) return;

                effectSource.clip = clip;
                effectSource.Play();
                break;
            case SourceAudio.UI:
                if (SoundVolume == 0) return;
                soundUISource.clip = clip;
                soundUISource.Play();
                break;
        }
    }

    public void PauseBGMusic()
    {
        musicSource.Pause();
    }

    public void ResumeBGMusic()
    {
        musicSource.UnPause();
    }

    //Used to play single sound clips.
    public void PlayOneShot(AudioClip clip, SourceAudio source = SourceAudio.Effect)
    {
        if (clip == null)
            return;
        switch (source)
        {
            case SourceAudio.Music:
                if (MusicVolume == 0) return;
                musicSource.PlayOneShot(clip);
                break;
            case SourceAudio.Effect:
                if (SoundVolume == 0) return;
                effectSource.PlayOneShot(clip);
                break;
        }
    }

    //RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
    public void RandomizeSfx(params AudioClip[] clips)
    {
        //Generate a random number between 0 and the length of our array of clips passed in.
        int randomIndex = Random.Range(0, clips.Length);

        //Choose a random pitch to play back our clip at between our high and low pitch ranges.
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        //Set the pitch of the audio source to the randomly chosen pitch.
        effectSource.pitch = randomPitch;

        //Set the clip to the clip at our randomly chosen index.
        effectSource.clip = clips[randomIndex];

        //Play the clip.
        effectSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || _currentMusic == clip)
            return;
        SetMasterVolume(MasterVolume);
        SetMusicVolume(MusicVolume);
        SetSoundVolume(SoundVolume);
        _currentMusic = clip;
        StopMusic();
        musicSource.clip = clip;
        musicSource.PlayDelayed(delayTime);
    }

    public void RandomizeMusic(params AudioClip[] clips)
    {
        //Generate a random number between 0 and the length of our array of clips passed in.
        int randomIndex = Random.Range(0, clips.Length);
        var clip = clips[randomIndex];
        if (clip == null || _currentMusic == clip)
            return;
        _currentMusic = clip;
        StopMusic();
        //Set the clip to the clip at our randomly chosen index.
        musicSource.clip = clips[randomIndex];

        //Play the clip.
        musicSource.PlayDelayed(delayTime);
    }

    public void PauseMusic()
    {
        //Play the clip.
        musicSource.Pause();
    }

    public void UnPauseMusic()
    {
        //Play the clip.
        musicSource.UnPause();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public bool IsMusicPlaying()
    {
        return musicSource.isPlaying;
    }


    private void SetMasterVolume(float volume)
    {
        // mixer.SetFloat("MasterVolume", volume);
        // PlayerPrefs.SetFloat(MASTER_KEY, volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        //PlayerPrefs.SetFloat(MUSIC_KEY, volume);
    }

    public void SetSoundVolume(float volume)
    {
        effectSource.volume = volume;
        // PlayerPrefs.SetFloat(SOUND_KEY, volume);
    }

    #endregion

    #region === Play Sound ===

    public void PlayWinSound()
    {
        if (!UseProfile.OnMusic)
            return;
        // musicSource.clip = winMusic;
        //musicSource.Play();
        PlaySingle(winMusic, SourceAudio.Effect);
    }

    public void PlayLoadingSceneBackgroundMusic()
    {
        musicSource.clip = LoadingSceneBGMusic;
        musicSource.Play();
    }

    public void PlayHomeSceneBackgroundMusic()
    {
        if (musicSource.isPlaying && musicSource.clip == HomeSceneBGMusic)
            return;

        musicSource.clip = HomeSceneBGMusic;
        musicSource.Play();
    }

    public void PlayGameplaySceneBackgroundMusic()
    {
        musicSource.clip = GameplayBGMusic;
        musicSource.Play();
    }

    public void PlayFunnyGameplaySceneBackgroundMusic()
    {
        musicSource.clip = FunnyGamePlayMusic;
        musicSource.Play();
    }

    public void PlaySadGameplaySceneBackgroundMusic()
    {
        musicSource.clip = SadGamePlayMusic;
        musicSource.Play();
    }

    public void PlayNoelSceneBackgroundMusic()
    {
        musicSource.clip = NoelPlayMusic;
        musicSource.Play();
    }

    public void PlayClickSound()
    {
        PlaySingle(clickSound, SourceAudio.UI);
    }

    public void PlayClick2Sound()
    {
        PlaySingle(clickSound2, SourceAudio.UI);
    }

    public void PlayClick3Sound()
    {
        PlaySingle(clickSound3, SourceAudio.UI);
    }

    public void PlayWrongDragSound()
    {
        PlaySingle(WrongtDragMusic, SourceAudio.Effect);
    }
    public void PlayCorrectDragSound()
    {
        PlaySingle(CorrectDragMusic, SourceAudio.Effect);
    }

    public void PlayDonePiecesSound()
    {
        PlaySingle(DonePieceMusic, SourceAudio.Effect);
    }

    public void PlayDoneStorySound()
    {
        PlaySingle(DonStoryMusic, SourceAudio.Effect);
    }

    public void PlayPigBankCoinAppearSound()
    {
        PlaySingle(pigBankCoinAppear, SourceAudio.UI);
    }

    public void PlayCoinEffectMoveSound()
    {
        PlaySingle(coinEffectMove, SourceAudio.UI);
    }
    public void PlayUiSoftToggleSound()
    {
        PlaySingle(uiSoftToggle, SourceAudio.UI);
    }

    public void PlayPieceDoneSound()
    {
        if (!UseProfile.OnSound)
            return;
        float tempVolume = soundUISource.volume;
        soundUISource.volume = tempVolume / 2f;
        PlaySingle(pieceDone, SourceAudio.UI);
        soundUISource.volume = tempVolume;
    }

    #endregion

    public void PlayOneShot(NameMusic name)
    {
        if (SoundVolume == 0) return;
        if (clips.ContainsKey(name))
        {
            if (clips[name].timmer <= 0)
            {
                clips[name].timmer = clips[name].spaceDelayTime;
                clips[name].isActiving = true;
                effectSource.clip = clips[name].audioClip;
                effectSource.Play();
            }
        }
    }

    public void PlayOneShot(NameMusic name, AudioClip clip)
    {
        if (SoundVolume == 0) return;
        if (clips.ContainsKey(name))
        {
            if (clips[name].timmer <= 0)
            {
                clips[name].timmer = clips[name].spaceDelayTime;
                clips[name].isActiving = true;
                effectSource.clip = clip;
                effectSource.Play();
            }
        }
    }

    public void PlayOneShot(NameMusic name, AudioClip clip, AudioSource source)
    {
        if (SoundVolume == 0) return;
        if (clips.ContainsKey(name))
        {
            if (clips[name].timmer <= 0)
            {
                clips[name].timmer = clips[name].spaceDelayTime;
                clips[name].isActiving = true;
                source.volume = 1;
                source.clip = clip;
                source.Play();
            }
        }
    }

    public void PlayOneShot(AudioClip clip, AudioSource source, float volume = 1)
    {
        if (SoundVolume == 0) return;

        source.volume = volume;
        source.clip = clip;
        source.Play();
    }

    private void Update()
    {
        ClipDelayHandle();
    }

    private void ClipDelayHandle()
    {
        if (clips == null)
            return;

        foreach (var clip in clips)
        {
            if (clip.Value.isActiving)
            {
                clip.Value.timmer -= Time.deltaTime;
                if (clip.Value.timmer < 0)
                {
                    clip.Value.timmer = 0;
                    clip.Value.isActiving = false;
                }
            }
        }
    }
}