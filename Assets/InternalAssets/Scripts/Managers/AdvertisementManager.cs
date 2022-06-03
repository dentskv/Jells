using System;
using GoogleMobileAds.Api;
using UnityEngine;
using Zenject;

public class AdvertisementManager : IInitializable, IDisposable
{
    private RewardedAd _rewardedAd;
    private InterstitialAd _interstitialAd;
    private AdRequest _request;
    private readonly string _adId = "ca-app-pub-3940256099942544/5224354917";

    public event Action OnEarnedReward;

    public void Initialize()
    {
        MobileAds.Initialize(status => {
            this._rewardedAd = new RewardedAd(_adId);
            this._interstitialAd = new InterstitialAd(_adId);
            
            this._rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
            this._rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
            this._rewardedAd.OnAdOpening += HandleRewardedAdOpening;
            this._rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
            this._rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
            this._rewardedAd.OnAdClosed += HandleRewardedAdClosed;

            this._interstitialAd.OnAdLoaded += HandleRewardedAdLoaded;
            this._interstitialAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
            this._interstitialAd.OnAdOpening += HandleRewardedAdOpening;
            this._interstitialAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
            this._interstitialAd.OnAdClosed += HandleRewardedAdClosed;
            
            _request = new AdRequest.Builder().Build();
            LoadRequest();
        });
    }

    public void Dispose()
    {
        this._rewardedAd.OnAdLoaded -= HandleRewardedAdLoaded;
        this._rewardedAd.OnAdFailedToLoad -= HandleRewardedAdFailedToLoad;
        this._rewardedAd.OnAdOpening -= HandleRewardedAdOpening;
        this._rewardedAd.OnAdFailedToShow -= HandleRewardedAdFailedToShow;
        this._rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
        this._rewardedAd.OnAdClosed -= HandleRewardedAdClosed;
        
        this._interstitialAd.OnAdLoaded -= HandleRewardedAdLoaded;
        this._interstitialAd.OnAdFailedToLoad -= HandleRewardedAdFailedToLoad;
        this._interstitialAd.OnAdOpening -= HandleRewardedAdOpening;
        this._interstitialAd.OnAdFailedToShow -= HandleRewardedAdFailedToShow;
        this._interstitialAd.OnAdClosed -= HandleRewardedAdClosed;
    }

    private void LoadRequest()
    {
        if(!_rewardedAd.IsLoaded()) this._rewardedAd.LoadAd(_request);
        if(!_interstitialAd.IsLoaded()) this._interstitialAd.LoadAd(_request);
    }

    private void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdLoaded event received");
    }

    private void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleRewardedAdFailedToLoad event received with message: " + args.LoadAdError);
    }

    private void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdOpening event received");
    }

    private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log("HandleRewardedAdFailedToShow event received with message: " + args.Message);
    }

    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        LoadRequest();
        Debug.Log("HandleRewardedAdClosed event received");
    }

    private void HandleUserEarnedReward(object sender, Reward args)
    {
        OnEarnedReward?.Invoke();
        Debug.Log("HandleRewardedAdRewarded event received for 100 coins");
    }
    
    public void ShowInterstitialAd()
    {
        if (_interstitialAd.IsLoaded())
        {
            this._interstitialAd.Show();
        }
        else
        {
            this._interstitialAd.LoadAd(_request);
        }
    }
    
    public void UserChoseToWatchAd()
    {
        if (this._rewardedAd.IsLoaded()) 
        {
            this._rewardedAd.Show();
        }
        else
        {
            LoadRequest();
        }
    }
}