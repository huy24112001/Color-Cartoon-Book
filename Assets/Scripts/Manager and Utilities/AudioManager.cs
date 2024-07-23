using UnityEngine;
using Lofelt.NiceVibrations;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [Space(10)]
    [SerializeField] AudioSource BGM;
    [SerializeField] AudioSource SFX;
    [SerializeField] AudioSource rustleLoopSFX;
    bool isRustleLoopPlaying;

    [Header("Audio Clips")]
    [Space(10)]
    [SerializeField] AudioClip[] BGMClips;
    [SerializeField] AudioClip UIButton;
    [SerializeField] AudioClip sparkle;
    [SerializeField] AudioClip chooseColor;
    [SerializeField] AudioClip congrats;
    [SerializeField] AudioClip fill;

    public AudioSource BackgroundMusic { get => BGM; set => BGM = value; }

    public static AudioManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance == this) return;
            Destroy(Instance.gameObject);
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    
    public void ChangeBGM(int order)
    {
        SFX.Stop();
        if (BGM.clip != BGMClips[order])
        {
            BGM.clip = BGMClips[order];
            BGM.loop = true;

            if (SettingsManager.Instance.MusicState == 1)
            {
                BGM.Play();
                BGM.mute = false;
            }
            else
            {
                BGM.Stop();
                BGM.mute = true;
            }
        }
    }

    public void StopBGM()
    {
        BGM.Stop();
        BGM.mute = true;
    }

    public void PlayBGM()
    {
        BGM.Play();
        BGM.mute = false;
    }

    public void PlayHaptic()
    {
        if (SettingsManager.Instance.VibrationState == 1) HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
    }
    
    #region =================== UI ===================

    public void Click()
    {
        GameController.Instance.admobAds.ShowInterstitialClick(false, "show inter from click AudioManager",
        actionIniterClose: () => {
            if (SettingsManager.Instance.SoundState == 1)
            {
                SFX.PlayOneShot(UIButton);
            }
        },
        actionIniterShow: () => {
            Debug.Log("Inter is show");
        });

       
    }
    
    public void Done()
    {
        GameController.Instance.admobAds.ShowInterstitialClick(false, "show inter from click Fullfill",
        actionIniterClose: () => {
            if (SettingsManager.Instance.SoundState == 1)
            {
                SFX.PlayOneShot(sparkle);
            }
        },
        actionIniterShow: () => {
            Debug.Log("Inter is show");
        });
      
    }

    public void ChooseColor()
    {
        if (SettingsManager.Instance.SoundState == 1)
        {
            SFX.PlayOneShot(chooseColor);
        }
    }

    public void Congrats()
    {
        if (SettingsManager.Instance.SoundState == 1)
        {
            SFX.PlayOneShot(congrats);
        }
    }

    public void Fill()
    {
        if (SettingsManager.Instance.SoundState == 1)
        {
            SFX.PlayOneShot(fill);
        }
    }

    public void PlayRustleLoop()
    {
        if (SettingsManager.Instance.SoundState == 1 && !isRustleLoopPlaying)
        {
            isRustleLoopPlaying = true;
            StartCoroutine(OnDelayPlayRustleLoop());
        }
    }

    IEnumerator OnDelayPlayRustleLoop()
    {
        yield return new WaitForEndOfFrame();
        rustleLoopSFX.Play();
    }

    public void StopRustleLoop()
    {
        rustleLoopSFX.Stop();
        isRustleLoopPlaying = false;
    }

    #endregion
}
