using System;
using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;
using com.PlugStudio.Patterns;

public class AdsManager : Singleton<AdsManager>
{
    public enum RewardType
    {
        Energy, Continue
    }

    private const string APP_ID             = "ca-app-pub-3117214092102716~2998811044";

    private const string BANNER_ID          = "ca-app-pub-3117214092102716/6839188987";
    private const string INTERSTITIAL_ID    = "ca-app-pub-3117214092102716/2511139154";
    private const string REWARD_ID          = "ca-app-pub-3117214092102716/9890935729";

    // 배너 광고
    private GoogleAdsBanner banner;
    // 전면 광고
    private GoogleAdsInterstitial inter;
    // 보상형 광고
    private GoogleAdsReward reward;

    private RewardType rewardType;
    private bool isCooldown;
    private bool loadedBanner;

    public bool LoadedReward { get { return reward.Loaded; } }
    public bool LoadedBanner { get { return loadedBanner; } }
    public float BannerHeight {
        get {
            if (loadedBanner == false)
                return 0;
            return banner.Height;
        }
    }

    private int PixelsToDp(int _pixel)
    {
        return (int)(_pixel / (Screen.dpi / 160));
    }

    private int DpToPixels(int _dp)
    {
        return (int)(_dp * (Screen.dpi / 160));
    }

    private void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;

            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });

        MobileAds.Initialize(APP_ID);

        AdSize bannerSize = new AdSize(AdSize.FullWidth, 50);

        int notchHeight = Display.main.systemHeight - Screen.height;

        if (notchHeight > 0)
        {
            notchHeight = Mathf.RoundToInt(PixelsToDp(notchHeight));
        }

        int y = Mathf.RoundToInt(PixelsToDp(Display.main.systemHeight)) * 2 - notchHeight;

        banner = new GoogleAdsBanner.Builder(BANNER_ID, bannerSize, 0, 0)
                  .SetOnFailedLaoded(BannerFailedToLoad)
                  .SetOnAdLaoded(BannerSuccessLoaded)
                  .Build();

        banner.SetPosition(0, y);

        inter = new GoogleAdsInterstitial.Builder(INTERSTITIAL_ID)
            .SetOnAdFailedToLoad(InterFailed)
            .SetOnAdClose(InterClosed)
            .Build();
        inter.Request();

        reward = new GoogleAdsReward.Builder(REWARD_ID)
            .SetOnAdFailedToLoad(RewardFailedToLoad)
            .SetOnAdClosed(RewardClosed)
            .SetOnAdRewarded(Rewarded)
            .Build();
        reward.Request();

        isCooldown = false;
    }

    public void ShowBanner()
    {
        banner.Request();
    }

    private FailedDialog failedDialog;

    public void ShowRewardContinue(FailedDialog _dialog)
    {
        rewardType = RewardType.Continue;
        failedDialog = _dialog;

        reward.Show();
    }

    public void ShowRewardEnergy()
    {
        rewardType = RewardType.Energy;
        reward.Show();
    }

    public void RequestReward()
    {
        if(LoadedReward)
        {
            return;
        }

        reward.Request();
    }

    public void ShowInterstitial()
    {
        if(isCooldown)
        {
            return;
        }

        isCooldown = true;

        inter.Request();
        inter.Show();

        StartCoroutine(InterstitalAdCooldown());
    }

    private void InterClosed(object sender, EventArgs args)
    {
        inter.Request();
    }

    private void InterFailed(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("## Interstitial Ad Load Failed ## " + args.Message);
    }

    private void RewardClosed(object sender, EventArgs args)
    {
        reward.Request();
    }

    private void RewardFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("## Reward Vidio Load Failed ## " + args.Message);
    }

    private void Rewarded(object sender, Reward args)
    {
        switch(rewardType)
        {
            case RewardType.Continue:
                if (failedDialog != null)
                {
                    failedDialog.gameObject.SetActive(false);
                    failedDialog = null;
                }

                GameManager.Instance.ContinueGame();
                Debug.Log("continue reward");
                break;
            case RewardType.Energy:
                SkillManager.Instance.AddEnergy();
                Debug.Log("energy reward");
                break;
        }

    }

    public void BannerFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.LogFormat("## Banner Failed to load : {0} ##", args.Message);

        loadedBanner = false;
    }

    public void BannerSuccessLoaded(object sender, EventArgs args)
    {
        loadedBanner = true;
    }

    private IEnumerator InterstitalAdCooldown()
    {
        float time = GameConst.Cooldown_InterstitialAd;

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        isCooldown = false;
    }
}
