using System;
using UnityEngine;
using GoogleMobileAds.Api;
using com.PlugStudio.Patterns;

public class AdsManager : Singleton<AdsManager>
{
    private const string APP_ID             = "ca-app-pub-3117214092102716~2998811044";

    private const string BANNER_ID          = "ca-app-pub-3117214092102716/6839188987";
    private const string INTERSTITIAL_ID    = "ca-app-pub-3117214092102716/2511139154";
    private const string REWARD_CONTINUE_ID = "ca-app-pub-3117214092102716/9890935729";
    private const string REWARD_ENERGY_ID   = "ca-app-pub-3117214092102716/7155320462";

    // 배너 광고
    private GoogleAdsBanner banner;
    // 전면 광고
    private GoogleAdsInterstitial inter;
    // 보상형 광고
    private GoogleAdsReward rewardContinue;
    private GoogleAdsReward rewardEnergy;

    private void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;

            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp, i.e.
                //   app = Firebase.FirebaseApp.DefaultInstance;
                // where app is a Firebase.FirebaseApp property of your application class.

                // Set a flag here indicating that Firebase is ready to use by your
                // application.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

        MobileAds.Initialize(APP_ID);

        AdSize bannerSize = new AdSize(AdSize.FullWidth, 50);

        banner = new GoogleAdsBanner.Builder(BANNER_ID, bannerSize)
                  .SetTestMode(true)
                  .SetOnFailedLaoded(BannerFailedToLoad)
                  .Build();

        inter = new GoogleAdsInterstitial.Builder(INTERSTITIAL_ID)
            .SetTestMode(true)
            .SetOnAdFailedToLoad(InterFailed)
            .SetOnAdClose(InterClosed)
            .Build();

        inter.Request();

        rewardContinue = new GoogleAdsReward.Builder(REWARD_CONTINUE_ID)
            .TestMode(true)
            .SetOnAdClosed(RewardClosed)
            .SetOnAdFailedToLoad(RewardFailedToLoad)
            .SetOnAdRewarded(RewardContinueRewarded)
            .Build();

        rewardContinue.Request();

        rewardEnergy = new GoogleAdsReward.Builder(REWARD_ENERGY_ID)
            .TestMode(true)
            .SetOnAdClosed(RewardClosed)
            .SetOnAdFailedToLoad(RewardFailedToLoad)
            .SetOnAdRewarded(RewardEnergyRewarded)
            .Build();
        rewardEnergy.Request();
    }

    public void ShowBanner()
    {
        banner.Request();
    }

    public void ShowRewardContinue()
    {
        rewardContinue.Show();
    }

    public void ShowRewardEnergy()
    {
        rewardEnergy.Show();
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
        rewardContinue.Request();
        rewardEnergy.Request();
    }

    private void RewardFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("## Reward Ad Load Failed");
    }

    private void RewardContinueRewarded(object sender, Reward args)
    {
        Debug.Log("## reward type :" + args.Type + " / amount :" + args.Amount);
    }

    private void RewardEnergyRewarded(object sender, Reward args)
    {
        SkillManager.Instance.AddEnergy();
    }

    public void BannerFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }
}
