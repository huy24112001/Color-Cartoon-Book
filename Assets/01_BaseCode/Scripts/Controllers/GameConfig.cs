using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameConfig : MonoBehaviour
{
    public static GameConfig Instance;

    [HideInInspector] public int coolDownInterAds = 15;
    [HideInInspector] public int coolDownInterAdsInGame = 100;
    [HideInInspector] public int coolDownOpenAds = 30;

    private void Awake()
    {
        Instance = this;
        coolDownInterAds = 15;
        coolDownInterAdsInGame = 100;
        coolDownOpenAds = 30;
    }

    private void Start()
    {
        
    }
}