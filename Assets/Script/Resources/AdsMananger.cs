using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AdsMananger : MonoBehaviour
{
    public string SDK_key;
    public string banner_AdUnitId; // Retrieve the ID from your account
    public string Interstitial_AdUnitId;
    public string Rerwarded_AdUnitId;
    int retryAttempt;
    int temp;
   
    public Action acClose, acRewarded;

    public bool bannerCheck;
    private Action acCloseInter;

    private static AdsMananger instance;
    private void Awake()
    {
        if (FindObjectsOfType(typeof(AdsMananger)).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public static AdsMananger Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AdsMananger>();
                if (!instance)
                {
                    instance = Instantiate(Resources.Load<AdsMananger>("AdsMananger"));
                }
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitAds();
      //  MaxSdk.ShowBanner(banner_AdUnitId);
        //bannerCheck = true;
    }
    public int GetBannerHeight()
    {
        if (MaxSdkUtils.IsTablet())
        {
            return Mathf.RoundToInt(90 * Screen.dpi / 160);
        }
        else
        {
            if (Screen.height <= 400 * Mathf.RoundToInt(Screen.dpi / 160))
            {
                return 32 * Mathf.RoundToInt(Screen.dpi / 160);
            }
            else if (Screen.height <= 720 * Mathf.RoundToInt(Screen.dpi / 160))
            {
                return 42 * Mathf.RoundToInt(Screen.dpi / 160);
            }
            else
            {
                return 50 * Mathf.RoundToInt(Screen.dpi / 160);
            }
        }
    }
    private void OnEnable()
    {

        InitializeInterstitialAds();
      //  InitializeRewardedAds();
    }

    private void InitAds()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            // AppLovin SDK is initialized, start loading ads          
        };

        MaxSdk.SetSdkKey(SDK_key);
        MaxSdk.SetUserId("USER_ID");
        MaxSdk.InitializeSdk();
     //   Debug.Log("Ads ");


        InitializeBannerAds();
      
        // Load the first rewarded ad
       
    }
    #region InitBanner
    public void InitializeBannerAds()
    {
        // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
        // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
        MaxSdk.CreateBanner(banner_AdUnitId, MaxSdkBase.BannerPosition.TopCenter);

        // Set background or background color for banners to be fully functional
    //    MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, Color.black);

        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
        MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;
    }

    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) { }

    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        //MaxSdk.HideBanner(adUnitId);
    }

    #endregion

    #region InitInter
    public void InitializeInterstitialAds()
    {
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

        // Load the first interstitial
        LoadInterstitial();
    }

    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(Interstitial_AdUnitId);
      //  Debug.Log("Init Intern");
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'     
        // Reset retry attempt    
        retryAttempt = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        acCloseInter?.Invoke();
        // Interstitial ad is hidden. Pre-load the next ad.
        LoadInterstitial();
    }

    public void ShowInterstitial(Action Close_CallBack = null)
    {
        if (MaxSdk.IsInterstitialReady(Interstitial_AdUnitId))
        {
            acCloseInter = Close_CallBack;
            MaxSdk.ShowInterstitial(Interstitial_AdUnitId);
         //   Debug.Log("Show inter");
        }
        else
        {
            LoadInterstitial();
        }
    }
    #endregion

    #region Init Rewarded
    public void InitializeRewardedAds()
    {
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(Rerwarded_AdUnitId);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

        retryAttempt = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        LoadRewardedAd();
    }
    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
    }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        acClose?.Invoke();
        LoadRewardedAd();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.
        acRewarded?.Invoke();
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
    }

    bool rewardedVideoAvailability;

    public void ShowRewardedAd(Action rewarded_CallBack, Action Close_CallBack, Action ClickAds_CallBack = null)
    {
        if (MaxSdk.IsRewardedAdReady(Rerwarded_AdUnitId))
        {
            MaxSdk.ShowRewardedAd(Rerwarded_AdUnitId);
            acRewarded = rewarded_CallBack;
            acClose = Close_CallBack;

        }
        else
        {
            LoadRewardedAd();
        }
    }

    #endregion
}
