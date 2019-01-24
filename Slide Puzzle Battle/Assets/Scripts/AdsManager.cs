using System;
using UnityEngine;
using GoogleMobileAds.Api;
using com.PlugStudio.Patterns;

public class AdsManager : Singleton<AdsManager>
{
    private const string appId = "ca-app-pub-3117214092102716~2998811044";
    private const string interUnit = "ca-app-pub-3117214 092102716/2511139154";
    private const string rewardUnit = "ca-app-pub-3117214092102716/9890935729";
    private const string bannerId = "ca-app-pub-3117214092102716/6839188987";

    // 배너 광고
    private GoogleAdsBanner banner;
    // 전면 광고
    private GoogleAdsInterstitial inter;
    // 보상형 광고
    private GoogleAdsReward reward;

    private void Awake()
    {
        MobileAds.Initialize(appId);
    }

    private void Start()
    {
        AdSize bannerSize = new AdSize(AdSize.FullWidth, 50);

        banner = new GoogleAdsBanner.Builder(bannerId, bannerSize)
               .SetTestMode(true)
               .Build();
        banner.Request();

        inter = new GoogleAdsInterstitial.Builder(interUnit)
            .SetTestMode(true)
            .SetOnAdFailedToLoad(InterFailed)
            .SetOnAdClose(InterClosed)
            .Build();

        inter.Request();

        reward = new GoogleAdsReward.Builder(rewardUnit)
            .TestMode(true)
            .SetOnAdClosed(RewardClosed)
            .SetOnAdFailedToLoad(RewardFailedToLoad)
            .SetOnAdRewarded(RewardRewarded)
            .Build();

        reward.Request();

    }

    public void ShowReward()
    {
        reward.Show();
    }

    public void ShowInterstitial()
    {
        inter.Show();
    }

    private void InterFailed(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("## Interstitial Ad Load Failed");
    }

    private void InterClosed(object sender, EventArgs args)
    {
        inter.Request();
    }

    private void RewardClosed(object sender, EventArgs args)
    {
        reward.Request();
    }

    private void RewardFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("## Reward Ad Load Failed");
    }

    private void RewardRewarded(object sender, Reward args)
    {
        Debug.Log("## reward type :" + args.Type + " / amount :" + args.Amount);
    }
}
