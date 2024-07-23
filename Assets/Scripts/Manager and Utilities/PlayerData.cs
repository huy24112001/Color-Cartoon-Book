using System;
using UnityEngine;

public class PlayerData
{
    public static long Coin
    {
        get => (long)PlayerPrefs.GetFloat("Coin", 0);
        set
        {
            long addOrMinus = value - Coin;
            PlayerPrefs.SetFloat("Coin", value);
            OnCoinChange?.Invoke(value, addOrMinus);
        }
    }

    public static event Action<long, long> OnCoinChange;

    public static long TotalCoin
    {
        get => (long)PlayerPrefs.GetFloat("TotalCoin", 0);
        set
        {
            var addOrMinus = value - TotalCoin;
            PlayerPrefs.SetFloat("TotalCoin", value);
            OnTotalCoinChange?.Invoke(value, addOrMinus);
        }
    }

    public static event Action<long, long> OnTotalCoinChange;

    public static int Language
    {
        get => PlayerPrefs.GetInt("Language", 0);
        set => PlayerPrefs.SetInt("Language", value);
    }

    public static int Music
    {
        get => PlayerPrefs.GetInt("Music", 1);
        set => PlayerPrefs.SetInt("Music", value);
    }

    public static int Sound
    {
        get => PlayerPrefs.GetInt("Sound", 1);
        set => PlayerPrefs.SetInt("Sound", value);
    }

    public static int Vibration
    {
        get => PlayerPrefs.GetInt("Vibration", 1);
        set => PlayerPrefs.SetInt("Vibration", value);
    }

    public static int BGM
    {
        get => PlayerPrefs.GetInt("BGM", 0);
        set => PlayerPrefs.SetInt("BGM", value);
    }

    public static int HasPlayed
    {
        get => PlayerPrefs.GetInt("HasPlayed", 0);
        set => PlayerPrefs.SetInt("HasPlayed", value);
    }

    public static int CurrentLevel
    {
        get => PlayerPrefs.GetInt("CurrentLevel", 1);
        set => PlayerPrefs.SetInt("CurrentLevel", value);
    }

    public static float CurrentPenProgress
    {
        get => PlayerPrefs.GetFloat("CurrentPenProgress", 0);
        set => PlayerPrefs.SetFloat("CurrentPenProgress", value);
    }

    public static int TotalPenUnlocked
    {
        get => PlayerPrefs.GetInt("TotalPenUnlocked", 0);
        set => PlayerPrefs.SetInt("TotalPenUnlocked", value);
    }

    public static int UsingPen
    {
        get => PlayerPrefs.GetInt("UsingPen", 0);
        set => PlayerPrefs.SetInt("UsingPen", value);
    }

    public static int OpenLastPen
    {
        get => PlayerPrefs.GetInt("OpenLastPen", 0);
        set => PlayerPrefs.SetInt("OpenLastPen", value);
    }

    public static int HighestLevel
    {
        get => PlayerPrefs.GetInt("HighestLevel", 1);
        set => PlayerPrefs.SetInt("HighestLevel", value);
    }
}
