using System;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
//using com.PlugStudio.Ads;

public class test : MonoBehaviour
{
    string interUnit = "ca-app-pub-3117214092102716/2511139154";
    string rewardUnit = "ca-app-pub-3117214092102716/9890935729";

    private GoogleAdsInterstitial inter;
    private GoogleAdsReward reward;

    private void Start()
    {
        MobileAds.Initialize("ca-app-pub-3117214092102716~2998811044");

        //ca-app-pub-3117214092102716/2511139154

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
