﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    private BannerView bannerBtmView;
    private BannerView bannerTopView;

    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    private float deltaTime = 0.0f;
    private static string outputMessage = string.Empty;

    private void Awake()
    {
        Instance = this;
    }

    public static string OutputMessage
    {
        set { outputMessage = value; }
    }

    public void Set_Ads()
    {

#if UNITY_ANDROID
        string appId = "ca-app-pub-4682698622407711~4209180854";
#elif UNITY_IOS
        string appId = "ca-app-pub-4682698622407711~8086445481";
#else
        string appId = "unexpected_platform";
#endif

        //MobileAds.SetiOSAppPauseOnBackground(true);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });


        this.RequestRewardedAd();
        this.RequestInterstitial();
        this.RequestBtmBanner();
        this.RequestTopBanner();

    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .AddKeyword("game")
            .SetGender(Gender.Male)
            .SetBirthday(new DateTime(1985, 1, 1))
            .TagForChildDirectedTreatment(false)
            .AddExtra("color_bg", "9B30FF")
            .Build();
    }

    #region Banner

    private void RequestBtmBanner()
    {
        // These ad units are configured to always serve test ads.

#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-4682698622407711/4585067879";
#elif UNITY_IOS
        string adUnitId = "ca-app-pub-4682698622407711/6899266562";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        this.bannerBtmView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Register for ad events.
        this.bannerBtmView.OnAdLoaded += this.HandleAdLoaded;
        this.bannerBtmView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
        this.bannerBtmView.OnAdOpening += this.HandleAdOpened;
        this.bannerBtmView.OnAdClosed += this.HandleAdClosed;
        this.bannerBtmView.OnAdLeavingApplication += this.HandleAdLeftApplication;
        Debug.Log("하단 배너");
        // Load a banner ad.
        this.bannerBtmView.LoadAd(this.CreateAdRequest());
    }

    private void RequestTopBanner()
    {
        // These ad units are configured to always serve test ads.

#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-4682698622407711/1223924866";
#elif UNITY_IOS
        string adUnitId = "ca-app-pub-4682698622407711/3143854389";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        this.bannerTopView = new BannerView(adUnitId, AdSize.MediumRectangle, AdPosition.Top);

        // Register for ad events.
        this.bannerTopView.OnAdLoaded += this.HandleAdLoaded;
        this.bannerTopView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
        this.bannerTopView.OnAdOpening += this.HandleAdOpened;
        this.bannerTopView.OnAdClosed += this.HandleAdClosed;
        this.bannerTopView.OnAdLeavingApplication += this.HandleAdLeftApplication;
        Debug.Log("상단 배너");

        // Load a banner ad.
        this.bannerTopView.LoadAd(this.CreateAdRequest());
        BannerShow(true);
    }

    public void BannerShow(bool isActive)
    {
        if (DataManager.Instance.state_Player.noAds)
        {
            bannerTopView.Hide();
            bannerBtmView.Hide();
            return;

        }

        if (isActive)
        {
            bannerBtmView.Show();
            bannerTopView.Hide();
        }
        else
        {
            bannerBtmView.Hide();
            bannerTopView.Show();
        }
    }

    #endregion

    #region Reward

    private void RequestRewardedAd()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-4682698622407711/1032353173";
#elif UNITY_IOS
        string adUnitId = "ca-app-pub-4682698622407711/5268710459";
#else
        string adUnitId = "unexpected_platform";
#endif
        // Create new rewarded ad instance.
        this.rewardedAd = new RewardedAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = this.CreateAdRequest();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void ShowRewardedAd()
    {
        if (!UIManager.Instance.Check_Network())
            return;

        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
        else
        {
            MonoBehaviour.print("Rewarded ad is not ready yet");
        }
    }

    #endregion

    #region interstitial

    private void RequestInterstitial()
    {
        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-4682698622407711/3658516516";
#elif UNITY_IOS
        string adUnitId = "ca-app-pub-4682698622407711/7895422530";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up interstitial ad before creating a new one.
        if (this.interstitial != null)
        {
            this.interstitial.Destroy(); 
        }

        // Create an interstitial.
        this.interstitial = new InterstitialAd(adUnitId);

        // Register for ad events.
        this.interstitial.OnAdLoaded += this.HandleInterstitialLoaded;
        this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
        this.interstitial.OnAdOpening += this.HandleInterstitialOpened;
        this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
        this.interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;

        // Load an interstitial ad.
        this.interstitial.LoadAd(this.CreateAdRequest());
    }

    public void ShowInterstitial()
    {
        if (!UIManager.Instance.Check_Network() || DataManager.Instance.state_Player.noAds)
            return;

        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
        else
        {
            MonoBehaviour.print("Interstitial is not ready yet");
        }
    }

    #endregion

    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("배너 로드 성공");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("배너 로드 실패");

    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
    }

    #endregion

    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialLoaded event received");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialOpened event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialClosed event received");
        RequestInterstitial();
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialLeftApplication event received");
    }

    #endregion

    #region RewardedAd callback handlers

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: " + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: " + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
        RequestRewardedAd();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);

        switch (UIManager.Instance.enum_Gift)
        {
            case Enum_Gift.Main_Gift:
                UIManager.Instance.Get_Gift(true);

                break;
            case Enum_Gift.Push_Gift:
                UIManager.Instance.Get_Push_Gift(true);

                break;
            case Enum_Gift.Shop_Gift:
                UIManager.Instance.Get_Shop_Gift_Item();
                break;
     
            default:
                break;
        }

    }

    #endregion
}
