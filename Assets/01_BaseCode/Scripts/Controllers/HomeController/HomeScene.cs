using MoreMountains.NiceVibrations;
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using UnityEngine.Serialization;

public class HomeScene : BaseScene
{
    public static HomeScene Instance;
    public Canvas homeCanvas;


    [SerializeField] private GameObject[] objTab;
    private bool _isNeedToFetchLeaderboard = true;
    public GameObject jigsawGiftPrefabs;


    [SerializeField] private GameObject shieldGO;
    //TODO: Update this later

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        
    }

    

    private void OnSelectStory()
    {
        SelectTable(1);

    }
    

    private void OnJigsawButtonClick()
    {
        SelectTable(0);
    }
    
    private void OnDailyButtonClick()
    {
        SelectTable(2);
    }
    
    private void OnMyWorkButtonClick()
    {
        SelectTable(2);
    }

    

    /// <summary>
    /// Hàm gọi khi stack Box = 0
    /// </summary>
    public override void OnEscapeWhenStackBoxEmpty()
    {
        //Hiển thị popup bạn có muốn thoát game ko?
    }
    

    public void SelectTable(int index)
    {
        
    }
    public void InitHelpStoryIcon()
    {
        
    }
    public void ShowGiftJigsaw()
    {
        
    }
    
    public IEnumerator IEActiveShield()
    {
        shieldGO.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        shieldGO.SetActive(false);
    }
}