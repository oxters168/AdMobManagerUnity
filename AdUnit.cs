using UnityEngine;
using GoogleMobileAds.Api;
using System;

[Serializable]
public class AdUnit
{
    public enum AdType { banner, interstitial }
    public enum AdSizeRepeat { custom, banner, iabBanner, leaderboard, mediumRectangle, smartBanner, }

    private BannerView bannerAd;
    private InterstitialAd interstitialAd;

    public AdType adType;
    public string adId;
    public bool startShown;

    [Space(10), Header("Banner Ad Options")]
    public AdPosition bannerAdPosition = AdPosition.Bottom;
    public AdSizeRepeat bannerAdSize = AdSizeRepeat.banner;
    [Tooltip("Specifies the banner ad dimensions if banner ad size is set to custom")]
    public Vector2Int customAdSize;

    public void Initialize(bool test)
    {
        string customId;
        if (adType == AdType.banner)
        {
            if (test)
            {
                #if UNITY_ANDROID
                    customId = "ca-app-pub-3940256099942544/6300978111";
                #elif UNITY_IPHONE
                    customId = "ca-app-pub-3940256099942544/2934735716";
                #else
                    customId = "unexpected_platform";
                #endif
            }
            else
                customId = adId;
        }
        else //Else is interstitial
        {
            if (test)
            {
                #if UNITY_ANDROID
                    customId = "ca-app-pub-3940256099942544/1033173712";
                #elif UNITY_IPHONE
                    customId = "ca-app-pub-3940256099942544/4411468910";
                #else
                    customId = "unexpected_platform";
                #endif
            }
            else
                customId = adId;
        }

        Initialize(customId);
    }
    public void Initialize(string customId)
    {
        adId = customId;

        if (adType == AdType.banner)
        {
            if (bannerAd != null)
                Destroy();

            AdSize currentSize;
            switch (bannerAdSize)
            {
                case AdSizeRepeat.banner:
                    currentSize = AdSize.Banner;
                    break;
                case AdSizeRepeat.iabBanner:
                    currentSize = AdSize.IABBanner;
                    break;
                case AdSizeRepeat.leaderboard:
                    currentSize = AdSize.Leaderboard;
                    break;
                case AdSizeRepeat.mediumRectangle:
                    currentSize = AdSize.MediumRectangle;
                    break;
                case AdSizeRepeat.smartBanner:
                    currentSize = AdSize.SmartBanner;
                    break;
                default:
                    currentSize = new AdSize(customAdSize.x, customAdSize.y);
                    break;
            }
            bannerAd = new BannerView(adId, currentSize, bannerAdPosition);
        }
        else if (adType == AdType.interstitial)
        {
            if (interstitialAd != null)
                Destroy();

            interstitialAd = new InterstitialAd(adId);
            interstitialAd.OnAdClosed += InterstitialAd_OnAdClosed;
            interstitialAd.OnAdFailedToLoad += InterstitialAd_OnAdFailedToLoad;
        }

        ReloadAd();

        if (startShown)
            Show();
    }

    private void InterstitialAd_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        ReloadAd();
    }
    private void InterstitialAd_OnAdClosed(object sender, EventArgs e)
    {
        ReloadAd();
    }

    public void Destroy()
    {
        if (adType == AdType.banner)
        {
            bannerAd.Destroy();
        }
        else if (adType == AdType.interstitial)
        {
            interstitialAd.OnAdFailedToLoad -= InterstitialAd_OnAdFailedToLoad;
            interstitialAd.OnAdClosed -= InterstitialAd_OnAdClosed;
            interstitialAd.Destroy();
        }
    }
    public void ReloadAd()
    {
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        if (adType == AdType.banner)
        {
            bannerAd.LoadAd(request);
        }
        else if (adType == AdType.interstitial)
        {
            interstitialAd.LoadAd(request);
        }
    }
    public void Show()
    {
        if (adType == AdType.banner)
            bannerAd.Show();
        else if (adType == AdType.interstitial && interstitialAd.IsLoaded())
            interstitialAd.Show();
    }
    public void Hide()
    {
        if (adType == AdType.banner)
            bannerAd.Hide();
    }
}
