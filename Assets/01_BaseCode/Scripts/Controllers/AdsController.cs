using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
//using com.adjust.sdk;
//using Facebook.Unity;
using Firebase.Analytics;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using UniRx;

public class AdsController : MonoBehaviour
{
    public static bool isAdmobInitDone;

#if UNITY_ANDROID
    private const string MaxSdkKey =
        "eQt0q3679KmUyKeNcSzqC01eB-lILmfTnJoufGxpSn__n1NVhHLeMgxZOaICke451El4ZBfuZum9Qw4WxzpW52";

    private const string InterstitialAdUnitId = "bcd5b9dfaba5174d";
    private const string RewardedAdUnitId = "d44c66a41a2ecc3a";
    private const string BanerAdUnitId = "40a51f77b0ae7c23";
#elif UNITY_IOS
    private const string MaxSdkKey =
 "eQt0q3679KmUyKeNcSzqC01eB-lILmfTnJoufGxpSn__n1NVhHLeMgxZOaICke451El4ZBfuZum9Qw4WxzpW52";
    private const string InterstitialAdUnitId = "ad8b53eb21de778a";
    private const string RewardedAdUnitId = "5cee1ddb1407475c";
    private const string BanerAdUnitId = "79fbc0e4c9e35762";
#endif
#if UNITY_ANDROID
    private string _adUnitIdHigh = "ca-app-pub-8467610367562059/9656627420";
    private string _adUnitIdMedium = "ca-app-pub-8467610367562059/2324932310";
    private string _adUnitIdLow = "ca-app-pub-8467610367562059/8893924685";
#elif UNITY_IPHONE
    private string _adUnitIdHigh = "ca-app-pub-3940256099942544/6978759866";
    private string _adUnitIdMedium = "ca-app-pub-3940256099942544/6978759866";
    private string _adUnitIdLow = "ca-app-pub-8467610367562059/4180939469";
#else
  private string _adUnitId = "unused";
#endif
    private RewardedInterstitialAd rewardedInterstitialAd;
    public DateTime countdownAds;
    bool isShowingAds;
    private bool _isInited;
    private IEnumerator reloadBannerCoru;
    public UnityAction actionInterstitialClose;
    private bool _isLoading;
    private UnityAction _actionClose;
    private UnityAction _actionRewardVideo;
    private UnityAction _actionNotLoadedVideo;
    private string actionWatchVideo;
    //public NativeAdsManager adsManager;
    private bool isGamePlay = false;
    public void Init()
    {
        ResetCoolDownTime();
        countdownAdsclick = 0;

        #region Applovin Ads

        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            Debug.Log("MAX SDK Initialized");
            InitInterstitial();
            InitRewardVideo();
            //InitializeBannerAds();

            // MaxSdk.ShowMediationDebugger();
            
            InitializeBannerMax();
        };
        MaxSdk.SetHasUserConsent(UseProfile.IsTrackedPremission);
        MaxSdk.SetVerboseLogging(false);
        MaxSdk.SetSdkKey(MaxSdkKey);
        MaxSdk.InitializeSdk();

        #endregion

        _isInited = true;

        //#if !UNITY_EDITOR
        Debug.Log("===== Init Admob ====");
        MobileAds.Initialize((initStatus) =>
        {
            Debug.Log("===== Init Admob Done ====");
            AppOpenAdManager.Instance.LoadAd(() =>
            {
                //if (SceneManager.GetActiveScene().name == SceneName.LOADING_SCENE)
                //{
                //}
            });
            InitBannerAdmob();
            AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
            isAdmobInitDone = true;
            //adsManager.OnInitAdmobDone();
            // LoadRewardedInterstitialAd();
        });
        //#endif
    }


    #region Admob Banner

    private BannerView _bannerView;
#if TESTER
    private const string BANNER_ADMOB_ID = "ca-app-pub-3940256099942544/9214589741"; //"ca-app-pub-3940256099942544/9214589741";
#else
    private const string BANNER_ADMOB_ID = "ca-app-pub-8467610367562059/8635310863";
#endif
    private bool _isShowAdmobBanner;
    private AdRequest bannerHomeLoadRequest;
    private DateTime lastRefreshCollapseBanner;

    public bool IsCooldownAdmobBanner
    {
        get => _isCooldownAdmobBanner;
        set
        {
            _isCooldownAdmobBanner = value;
            var cooldownAdmobBannerTimer =
                RemoteConfigController.GetFloatConfig(FirebaseConfig.MINIMUM_TIME_SHOW_COLLAPSE, 5);
            if (_cooldownAdmobBannerTimer < cooldownAdmobBannerTimer)
            {
                _cooldownAdmobBannerTimer =
                    cooldownAdmobBannerTimer;
            }
        }
    }

    private bool _isCooldownAdmobBanner;
    private bool _isTryLoadAdmobBanner;
    private double _cooldownAdmobBannerTimer;

    public void InitBannerAdmob()
    {
        LoadBannerAdmob();
       
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            Debug.Log("SceneManager " + scene.name);
            if (scene.name == "Loading Scene")
            {
                DestroyBanner();
            }
            else if (scene.name == "Gameplay")
            {
                isGamePlay= true;
                // RefreshCollapseBanner();
                ShowBanner();
            }
        };
    }


    /*public void RefreshCollapseBanner()
    {
        if (DateTime.Now.Subtract(lastRefreshCollapseBanner).TotalSeconds < 60)
            return;
        bannerHomeLoadRequest.Extras["collapsible_request_id"] = Guid.NewGuid().ToString();
        _bannerViewHome.LoadAd(bannerHomeLoadRequest);
    }*/

    private void RefreshAdmobBanner()
    {
        _isTryLoadAdmobBanner = true;
         if (!isGamePlay) return;
        _bannerView.LoadAd(bannerHomeLoadRequest);
    }

    public void LoadBannerAdmob()
    {
        AdSize adaptiveSize =
            AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        _bannerView?.Destroy();
        _bannerView = new BannerView(BANNER_ADMOB_ID, adaptiveSize, AdPosition.Bottom);
        //_bannerView.Hide();
        bannerHomeLoadRequest = new AdRequest();
        lastRefreshCollapseBanner = DateTime.Now;
        IsCooldownAdmobBanner = true;
        //Admob request
        bannerHomeLoadRequest.Extras.Add("collapsible", "bottom");
        bannerHomeLoadRequest.Extras.Add("collapsible_request_id", Guid.NewGuid().ToString());
        _isTryLoadAdmobBanner = true;
        _bannerView.LoadAd(bannerHomeLoadRequest);
        _bannerView.Hide();
        Debug.Log("LoadBannerAdmob");
        _bannerView.OnBannerAdLoaded += OnBannerAdLoaded;

        _bannerView.OnAdPaid += OnAdPaid;

        _bannerView.OnBannerAdLoadFailed += OnBannerAdLoadFailed;

        void OnBannerAdLoaded()
        {
            Debug.Log("OnBannerAdLoaded Success");
            _isTryLoadAdmobBanner = false;
            var isEnableAdmobBanner = RemoteConfigController.GetBoolConfig(FirebaseConfig.ENABLE_ADMOB_BANNER, true);
            _isShowAdmobBanner = isEnableAdmobBanner;
            _cooldownAdmobBannerTimer =
                RemoteConfigController.GetFloatConfig(FirebaseConfig.COOLDOWN_ADMOB_REFRESH, 40);
            ShowBanner();
        }

        void OnAdPaid(AdValue adValue)
        {
            var revenue = adValue.Value / 1000000d;
            AppFlyerGplay.LogRevenue("banner", "admob", "banner", "banner", revenue, adValue.CurrencyCode);
            try
            {
                List<Parameter> parameters = new List<Parameter>()
                {
                    new Parameter("ad_source", "admob"),
                    new Parameter("ad_format", "banner"),
                    new Parameter("value", revenue),
                    new Parameter("currency", adValue.CurrencyCode)
                };
                FirebaseAnalytics.LogEvent("ad_impression", parameters.ToArray());
                FirebaseAnalytics.LogEvent("sub_rev_fix", parameters.ToArray());
                FirebaseAnalytics.LogEvent("real_time_rev_fix", parameters.ToArray());
                FirebaseAnalytics.LogEvent("ad_revenue_sdk", parameters.ToArray());
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        void OnBannerAdLoadFailed(LoadAdError error)
        {
            Debug.Log("OnBannerAdLoadFailed Failed");

            _isTryLoadAdmobBanner = true;
            _cooldownAdmobBannerTimer =
                RemoteConfigController.GetFloatConfig(FirebaseConfig.COOLDOWN_ADMOB_REFRESH, 60);
        }
    }

    #endregion



    public void InitializeBannerMax()
    {

        MaxSdk.CreateBanner(BanerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
        MaxSdk.SetBannerBackgroundColor(BanerAdUnitId, Color.clear);
        MaxSdk.SetBannerExtraParameter(BanerAdUnitId, "adaptive_banner", "true");

        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
        MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;
        //ShowBanner();
    }

    private void OnBannerAdLoadedEvent(string obj, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX Banner loaded success");
        if (reloadBannerCoru != null)
        {
            StopCoroutine(reloadBannerCoru);
            reloadBannerCoru = null;
        }
    }

    private void OnBannerAdClickedEvent(string obj, MaxSdkBase.AdInfo adInfo)
    {
        //inter click
        Debug.Log("Click Baner !!!");
    }

    private void OnBannerAdLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo errorInfo)
    {
        if (reloadBannerCoru != null)
        {
            StopCoroutine(reloadBannerCoru);
            reloadBannerCoru = null;
        }

        reloadBannerCoru = Helper.StartAction(() => { ShowBanner(); }, 0.3f);
        StartCoroutine(reloadBannerCoru);
    }

    private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
    }

    private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
    }

    private void InitRewardVideo()
    {
        InitializeRewardedAds();
    }

    #region Interstitial

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo info)
    {
        _isLoading = true;
        GameController.Instance.AnalyticsController.LogInterReady();
    }

    private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo info)
    {
        Debug.Log("adUnitId : " + adUnitId + "errorcode :" + info.Code);
        _isLoading = false;
        actionInterstitialClose?.Invoke();
        actionInterstitialClose = null;
        Invoke("RequestInterstitial", 3);
    }
    void RefeshCloseAds()
    {
        isShowingAds = false;
    }

    private void RequestInterstitial()
    {
        if (_isLoading) return;

        MaxSdk.LoadInterstitial(InterstitialAdUnitId);
        GameController.Instance.AnalyticsController.LogInterLoad();
        _isLoading = true;
    }

    private void InterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo info, MaxSdkBase.AdInfo adInfo)
    {
        _isLoading = false;
        actionInterstitialClose?.Invoke();
        actionInterstitialClose = null;
        RequestInterstitial();
    }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo info)
    {
        _isLoading = false;
        Debug.Log("InterstitialAdClosedEvent");
        Time.timeScale = 1;

        _actionRewardVideo?.Invoke();
        _actionRewardVideo = null;

        _actionClose?.Invoke();
        _actionClose = null;

        actionInterstitialClose?.Invoke();
        actionInterstitialClose = null;
        countdownAdsclick = 0;
        RequestInterstitial();
        ResetCoolDownTime();
        Invoke("RefeshCloseAds", 1);

        //if (GamePlayControl.Instance != null)
        //{
        //    GamePlayControl.Instance.timer = 0;
        //}
    }

    public void ResetCoolDownTime()
    {
        countdownAdsclick = 0;
        countdownAds = DateTime.Now.AddSeconds(90);
        countdownAdsclick = 0;
        AppOpenAdManager.Instance.ResetCoolDownTime();
        //if (SceneManager.GetActiveScene().name == SceneName.GAME_PLAY) GameManager.Instance.ResetInterAdsCoolDown();
    }
    
    private void MaxSdkCallbacks_OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo info)
    {
        Debug.Log("InterstitialAdOpenedEvent");
        _isLoading = false;
        Time.timeScale = 0;
    }

    private void MaxSdkCallbacks_OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo info)
    {
        GameController.Instance.AnalyticsController.LogInterClick();
        _isLoading = false;
    }

    public bool ShowInterstitial(bool isShowImmediately = false, string actionWatchLog = "other",
        UnityAction actionIniterClose = null, UnityAction actionIniterShow = null, string level = null,
        bool isShowBreakAds = true)
    {
        Debug.Log("show inter ========" + countdownAds);
        if (UseProfile.IsRemoveAds /*|| UseProfile.IsVip*/)
        {
            actionIniterClose?.Invoke();
            return false;
        }

        if (countdownAds > DateTime.Now && !isShowImmediately)
        {
            actionIniterClose?.Invoke();
            Debug.Log("cooldowning");
            return false;
        }
        ShowInterstitialHandle(actionWatchLog, actionIniterClose);
        
        return true;
    }
    

    public bool ShowInterstitialClick(bool isShowImmediatly = false, string actionWatchLog = "other",
        UnityAction actionIniterClose = null, UnityAction actionIniterShow = null, string level = null,
        bool isInGame = false)
    {
        Debug.Log("show inter click 0 ========");

        if (UseProfile.IsRemoveAds /*UseProfile.IsVip*/)
        {
            if (!isInGame)
                actionIniterClose?.Invoke();
            return false;
        }

        if (countdownAdsclick > RemoteConfigController.GetFloatConfig(FirebaseConfig.DELAY_SHOW_INITSTIALL, 90) ||
            isShowImmediatly)
        {
            Debug.Log("show inter click ========");
            //#if UNITY_EDITOR
            //            countdownAds = 0;
            //            countdownAdsclick = 0;
            //            //return true;
            //#endif
            ShowInterstitialHandle(actionWatchLog, actionIniterClose, isInGame: isInGame);
        }
        else
        {
            if (!isInGame)
                if (actionIniterClose != null)
                    actionIniterClose();
        }

        //#if UNITY_EDITOR
        //        actionIniterClose?.Invoke();
        //        countdownAds = 0;
        //        //return true;
        //#endif
        return true;
    }

    private void FixedUpdate()
    {
        // UnityEngine.Debug.Log("Ready: " + IsLoadedInterstitial());
    }
    public bool IsLoadedInterstitial()
    {
        return MaxSdk.IsInterstitialReady(InterstitialAdUnitId);
    }

    private void ShowInterstitialHandle(string actionWatchLog = "other",
        UnityAction actionIniterClose = null, string level = null, bool isInGame = false)
    {
        actionIniterClose += () => { IsCooldownAdmobBanner = true; };
        if (IsLoadedInterstitial())
        {
            isShowingAds = true;
            if (isInGame)
            {
                Debug.Log("show inter ======");
                AdsBreakPopUp.Setup().Show(() =>
                {
                    this.actionInterstitialClose = actionIniterClose;
                    IsCooldownAdmobBanner = false;
                    MaxSdk.ShowInterstitial(InterstitialAdUnitId, actionWatchLog);

                    countdownAdsclick = 0;
                    GameController.Instance.AnalyticsController.LogInterShow(actionWatchLog);

                    UseProfile.NumberOfAdsInDay = UseProfile.NumberOfAdsInDay + 1;
                    UseProfile.NumberOfAdsInPlay = UseProfile.NumberOfAdsInPlay + 1;
                });
            }
            else
            {
                this.actionInterstitialClose = actionIniterClose;
                IsCooldownAdmobBanner = false;
                MaxSdk.ShowInterstitial(InterstitialAdUnitId, actionWatchLog);

                countdownAdsclick = 0;
                GameController.Instance.AnalyticsController.LogInterShow(actionWatchLog);

                UseProfile.NumberOfAdsInDay = UseProfile.NumberOfAdsInDay + 1;
                UseProfile.NumberOfAdsInPlay = UseProfile.NumberOfAdsInPlay + 1;
            }
        }
        else
        {
            if (!isInGame)
                if (actionIniterClose != null)
                    actionIniterClose();
            RequestInterstitial();
        }
    }

    private void InitInterstitial()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += MaxSdkCallbacks_OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += MaxSdkCallbacks_OnInterstitialDisplayedEvent;

        RequestInterstitial();

        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        // MaxSdkCallbacks.
    }

    public float countdownAdsclick;


    #endregion

    #region Video Reward

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(RewardedAdUnitId);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo info)
    {
        GameController.Instance.AnalyticsController.LogVideoRewardReady();
    }

    private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorCode)
    {
        Debug.Log("Rewarded ad failed to load with error code: " + errorCode.Code);
        Invoke("LoadRewardedAd", 15);
        GameController.Instance.AnalyticsController.LogVideoRewardLoadFail(actionWatchVideo.ToString(),
            errorCode.ToString());
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorCode, MaxSdkBase.AdInfo info)
    {
        Debug.Log("Rewarded ad failed to display with error code: " + errorCode.Code);
        isVideoDone = false;

        //if (IsLoadedInterstitial())
        //{
        //    ShowInterstitial(isShowImmediatly: true);
        //}
        //else
        //{
        //    //ConfirmBox.Setup().AddMessageYes(Localization.Get("s_noti"), Localization.Get("s_TryAgain"), () => { });
        //}
        LoadRewardedAd();
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo info)
    {
        Debug.Log("Rewarded ad displayed " + isVideoDone);
        isVideoDone = false;
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo info)
    {
        Debug.Log("Rewarded ad clicked");
        isVideoDone = true;
        GameController.Instance.AnalyticsController.LogClickToVideoReward(actionWatchVideo.ToString());
    }

    private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo info)
    {
        //if (GamePlayControl.Instance != null)
        //{
        //    GamePlayControl.Instance.timer = 0;
        //}

        // Rewarded ad is hidden. Pre-load the next ad
        Debug.Log("Rewarded ad dismissed");
        _actionClose?.Invoke();
        _actionClose = null;
        _actionRewardVideo = null;
        LoadRewardedAd();
    }

    bool isVideoDone;

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo info)
    {
        // Rewarded ad was displayed and user should receive the reward
        Debug.Log("Rewarded ad received reward");
        isShowingAds = false;
        isVideoDone = true;
        _actionRewardVideo?.Invoke();
        _actionRewardVideo = null;
        ResetCoolDownTime();
        countdownAdsclick = 0;
        GameController.Instance.AnalyticsController.LogVideoRewardShowDone(actionWatchVideo.ToString());
    }

    private void InitializeRewardedAds()
    {
        // Attach callbacks
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        // Load the first RewardedAd
        LoadRewardedAd();
    }

    public bool IsLoadedVideoReward()
    {
        var result = MaxSdk.IsRewardedAdReady(RewardedAdUnitId);
        if (!result)
        {
            RequestInterstitial();
        }

        return result;
    }

    /// <summary>
    /// Xử lý Show Video
    /// </summary>
    /// <param name="actionReward">Hành động khi xem xong Video và nhận thưởng </param>
    /// <param name="actionNotLoadedVideo"> Hành động báo lỗi không có video để xem </param>
    /// <param name="actionClose"> Hành động khi đóng video (Đóng lúc đang xem dở hoặc đã xem hết) </param>
    public bool ShowVideoReward(UnityAction actionReward, UnityAction actionNotLoadedVideo, UnityAction actionClose,
        string actionType, string level = "")
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            actionNotLoadedVideo?.Invoke();
            GameController.Instance.AnalyticsController.LogWatchVideo(actionType, true, false, level);
            return false;
        }

        actionWatchVideo = actionType;
        GameController.Instance.AnalyticsController.LogRequestVideoReward(actionType.ToString());
        GameController.Instance.AnalyticsController.LogVideoRewardEligible();
        if (IsLoadedVideoReward())
        {
            Debug.Log("Loaded Reward");
            isShowingAds = true;
            ResetCoolDownTime();
            
            countdownAdsclick = 0;
            this._actionNotLoadedVideo = actionNotLoadedVideo;
            this._actionClose = actionClose;
            this._actionRewardVideo = actionReward;

            MaxSdk.ShowRewardedAd(RewardedAdUnitId, actionType.ToString());
            GameController.Instance.AnalyticsController.LogWatchVideo(actionType, true, true, level);
            GameController.Instance.AnalyticsController.LogVideoRewardShow(actionWatchVideo.ToString());
        }
        else
        {
            Debug.LogError("Not Loaded Reward");
            if (IsLoadedInterstitial())
            {
                Debug.LogError("Show Inter");
                this._actionNotLoadedVideo = actionNotLoadedVideo;
                this._actionClose = actionClose;
                this._actionRewardVideo = actionReward;

                ShowInterstitial( false,"other", actionIniterClose: () =>
                {
                    Debug.LogError("showed inter");
                },null);
                GameController.Instance.AnalyticsController.LogWatchVideo(actionType, true, true, level);
                Debug.Log("ShowInterstitial !!!");
                ResetCoolDownTime();
                countdownAdsclick = 0;
                return true;
            }
            else
            {
                //ConfirmBox.Setup().AddMessageYes(Localization.Get("s_noti"), Localization.Get("s_TryAgain"), () => { });
                Debug.Log("No ads !!!");
                actionNotLoadedVideo?.Invoke();
                GameController.Instance.AnalyticsController.LogWatchVideo(actionType, false, true, level);
                return false;
            }
        }
        return true;
    }
    #endregion

    #region Banner

    public void ShowBanner()
    {
        // Đảm bảo đoạn mã chạy trên luồng chính
        MainThreadDispatcher.Post((state) =>
        {
            if (SceneManager.GetActiveScene().name == SceneName.LOADING_SCENE)
            {
                return;
            }
            if (UseProfile.IsRemoveAds)
                return;
            Debug.Log("ShowBanner " + _isShowAdmobBanner);
            if (_isShowAdmobBanner)
                ShowBannerAdmob();
            else
                ShowBannerMax();
        }, null);
     
    }

    public void ShowBannerAdmob()
    {
        MaxSdk.HideBanner(BanerAdUnitId);
        _bannerView?.Show();
    }

    public void ShowBannerMax()
    {
        _bannerView?.Hide();
        MaxSdk.ShowBanner(BanerAdUnitId);
    }

    public void DestroyBanner()
    {
        Debug.Log("destroy banner");
        MaxSdk.HideBanner(BanerAdUnitId);
        _bannerView?.Hide();
    }

    #endregion

    #region Open App Ads

    DateTime oldTime = DateTime.MinValue;

    public void OnAppStateChanged(AppState state)
    {
        // Đảm bảo đoạn mã chạy trên luồng chính
        MainThreadDispatcher.Post((state1) =>
        {
            if (isAdmobInitDone && SceneManager.GetActiveScene().name != "Loading Scene")
                if (state == AppState.Foreground && TimeManager.CaculateTime(oldTime, DateTime.Now) > 90 && !isShowingAds)
                {
                    // COMPLETE: Show an app open ad if available.
                    AppOpenAdManager.Instance.ShowAdIfAvailable();
                    oldTime = DateTime.Now;
                }
        }, null);
        
    }

    #endregion

    #region Reward Inter

    public void LoadRewardedInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (rewardedInterstitialAd != null)
        {
            rewardedInterstitialAd.Destroy();
            rewardedInterstitialAd = null;
        }

        Debug.Log("Loading the rewarded interstitial ad.");
        // create our request used to load the ad.
        var adRequest = new AdRequest();
        // send the request to load the ad.
        RewardedInterstitialAd.Load(_adUnitIdLow, adRequest,
            (RewardedInterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("rewarded interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    //LoadMoreRewardInter(_adUnitIdMedium);
                    return;
                }

                Debug.Log("Rewarded interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedInterstitialAd = ad;
                InitRegister(ad);
            });
    }
    //void LoadMoreRewardInter(string ID)
    //{
    //    var adRequest = new AdRequest.Builder().Build();
    //    // send the request to load the ad.
    //    RewardedInterstitialAd.Load(ID, adRequest,
    //        (RewardedInterstitialAd ad, LoadAdError error) =>
    //        {
    //            // if error is not null, the load request failed.
    //            if (error != null || ad == null)
    //            {
    //                Debug.LogError("rewarded interstitial ad failed to load an ad " +
    //                               "with error : " + error);
    //                LoadMoreRewardInter(_adUnitIdLow);
    //                return;
    //            }
    //            Debug.Log("Rewarded interstitial ad loaded with response : "
    //                      + ad.GetResponseInfo());

    //            rewardedInterstitialAd = ad;
    //            InitRegister(ad);
    //        });
    //}
    private UnityAction actionRewardInter;
    private UnityAction actionCloseRewardInter;
    private bool isRewardInter;


    private void HandleAdPaidEvent(AdValue adValue)
    {
        
        AppFlyerGplay.LogRevenue("reward_inter","admob","reward_inter", "reward_inter",
            adValue.Value / 1000000d, adValue.CurrencyCode);
        
        AnalyticsController.LogAdsRevenue("sub_rev_fix", "admob", "reward_inter", "reward_inter",
            adValue.Value / 1000000d, adValue.CurrencyCode);
        AnalyticsController.LogAdsRevenue("real_time_rev_fix", "admob", "reward_inter", "reward_inter",
            adValue.Value / 1000000d, adValue.CurrencyCode);
        AnalyticsController.LogAdsRevenue("ad_revenue_sdk", "admob", "reward_inter", "reward_inter",
            adValue.Value / 1000000d, adValue.CurrencyCode);
        AnalyticsController.LogAdsRevenue("ad_impression", "admob", "reward_inter", "reward_inter",
            adValue.Value / 1000000d, adValue.CurrencyCode);
    }

    void InitRegister(RewardedInterstitialAd ad)
    {
        ad.OnAdFullScreenContentClosed += OnAdContentclose;
        ad.OnAdPaid += HandleAdPaidEvent;
    }

    private void OnAdContentclose()
    {
        Debug.Log("=======close");
        Invoke("CloseAdReward", 0.1f);
        if (actionCloseRewardInter != null)
            actionCloseRewardInter.Invoke();
        LoadRewardedInterstitialAd();
    }

    void CloseAdReward()
    {
        if (isRewardInter)
            if (actionRewardInter != null)
            {
                actionRewardInter.Invoke();
                actionRewardInter = null;
            }

        isRewardInter = false;
    }

    private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo impressionData)
    {
        double revenue = impressionData.Revenue;
        
        AppFlyerGplay.LogRevenue(impressionData.Placement,"max",impressionData.AdUnitIdentifier,
            impressionData.AdFormat, revenue, "USD");
        
        AnalyticsController.LogAdsRevenue("ad_impression", impressionData.NetworkName, impressionData.AdUnitIdentifier,
            impressionData.AdFormat, revenue, "USD");
        AnalyticsController.LogAdsRevenue("ad_revenue_sdk", impressionData.NetworkName.ToLower(),
            impressionData.AdUnitIdentifier.ToLower(), impressionData.AdFormat.ToLower(), revenue, "USD");
        AnalyticsController.LogAdsRevenue("real_time_rev_fix", impressionData.NetworkName.ToLower(),
            impressionData.AdUnitIdentifier.ToLower(), impressionData.AdFormat.ToLower(), revenue, "USD");
        //GameController.Instance.AnalyticsController.LogRevenueDay01((float)revenue);

        Dictionary<string, object> paramas = new Dictionary<string, object>();
        //FB.LogPurchase((decimal)revenue, "USD", paramas);
    }

    #endregion

    private void Update()
    {
        if (_isShowAdmobBanner || _isTryLoadAdmobBanner)
        {
            if (IsCooldownAdmobBanner)
            {
                _cooldownAdmobBannerTimer -= Time.unscaledDeltaTime;
                if (_cooldownAdmobBannerTimer <= 0)
                {
                    RefreshAdmobBanner();
                }
            }
        }

        //if (GamePlayControl.Instance != null)
        //    countdownAds += Time.deltaTime;
        countdownAdsclick += Time.deltaTime;
    }
}