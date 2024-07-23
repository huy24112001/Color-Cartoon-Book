using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] GameObject soundOnBtn;
    [SerializeField] GameObject soundOffBtn;
    [SerializeField] GameObject musicOnBtn;
    [SerializeField] GameObject musicOffBtn;
    [SerializeField] GameObject vibrationOnBtn;
    [SerializeField] GameObject vibrationOffBtn;

    [SerializeField] int soundState, musicState, vibrationState;

    public int SoundState { get => soundState; set => soundState = value; }
    public int MusicState { get => musicState; set => musicState = value; }
    public int VibrationState { get => vibrationState; set => vibrationState = value; }

    public static SettingsManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(Instance.gameObject);
                Instance = this;
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        soundState = PlayerData.Sound;
        if (soundState == 0)
        {
            if (soundOffBtn != null) soundOffBtn.SetActive(true);
            if (soundOnBtn != null) soundOnBtn.SetActive(false);
        }
        else
        {
            if (soundOffBtn != null) soundOffBtn.SetActive(false);
            if (soundOnBtn != null) soundOnBtn.SetActive(true);
        }

        musicState = PlayerData.Music;
        if (musicState == 0)
        {
            if (musicOffBtn != null) musicOffBtn.SetActive(true);
            if (musicOnBtn != null) musicOnBtn.SetActive(false);
        }
        else
        {
            if (musicOffBtn != null) musicOffBtn.SetActive(false);
            if (musicOnBtn != null) musicOnBtn.SetActive(true);
        }
        AudioManager.Instance.BackgroundMusic.mute = musicState == 0;

        vibrationState = PlayerData.Vibration;
        if (vibrationState == 0)
        {
            if (vibrationOffBtn != null) vibrationOffBtn.SetActive(true);
            if (vibrationOnBtn != null) vibrationOnBtn.SetActive(false);
        }
        else
        {
            if (vibrationOffBtn != null) vibrationOffBtn.SetActive(false);
            if (vibrationOnBtn != null) vibrationOnBtn.SetActive(true);
        }
    }

    public void ToggleSound()
    {
        soundState = 1 - soundState;
        if (soundState == 0)
        {
            soundOffBtn.SetActive(true);
            soundOnBtn.SetActive(false);
        }
        else
        {
            soundOffBtn.SetActive(false);
            soundOnBtn.SetActive(true);
        }
        PlayerData.Sound = soundState;
        AudioManager.Instance.Click();
    }

    public void ToggleMusic()
    {
        musicState = 1 - musicState;
        if (musicState == 0)
        {
            musicOffBtn.SetActive(true);
            musicOnBtn.SetActive(false);
        }
        else
        {
            musicOffBtn.SetActive(false);
            musicOnBtn.SetActive(true);
        }
        AudioManager.Instance.BackgroundMusic.mute = musicState == 0;
        PlayerData.Music = musicState;
        AudioManager.Instance.Click();
    }

    public void ToggleVibration()
    {
        vibrationState = 1 - vibrationState;
        if (vibrationState == 0)
        {
            vibrationOffBtn.SetActive(true);
            vibrationOnBtn.SetActive(false);
        }
        else
        {
            vibrationOffBtn.SetActive(false);
            vibrationOnBtn.SetActive(true);
        }
        PlayerData.Vibration = vibrationState;
        AudioManager.Instance.Click();
    }
}
