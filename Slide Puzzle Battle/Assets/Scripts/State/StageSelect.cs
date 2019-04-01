using System.Collections;
using UnityEngine;
using com.PlugStudio.Patterns;

public class StageSelect : State
{
    public RectTransform contentView;
    public RectTransform settingView;
    public StageList listView;

    private static bool isInit = false;
    private static int beforeCompleteLevel = 0;

    public override IEnumerator Initialize(params object[] _data)
    {
#if UNITY_ANDROID
        //float height = Screen.height < 1920f ? 1920 : Screen.height;

        //if (AdsManager.Instance.LoadedBanner)
        //{
        //    height -= AdsManager.Instance.BannerHeight;
        //}

        //contentView.sizeDelta = new Vector3(0, height);
        //settingView.sizeDelta = contentView.sizeDelta;

        if (AdsManager.Instance.LoadedReward == false)
        {
            AdsManager.Instance.RequestReward();
        }
#endif

        yield return listView.Init(GameManager.Instance.Stages);
        yield return listView.SetScrollPosition(GameManager.Instance.LastPlayedLevel + 1, true);

        SoundManager.Instance.PlayBGM(0);
    }

    public override void Begin()
    {
        if(isInit == false)
        {
            isInit = true;

            //AdsManager.Instance.ShowBanner();

            beforeCompleteLevel = GameManager.Instance.CompleteLevel;
        }
        else
        {
            if (beforeCompleteLevel != GameManager.Instance.CompleteLevel)
            {
                beforeCompleteLevel = GameManager.Instance.CompleteLevel;

                listView.CheckClear();
            }
        }
    }

    public override void Execute()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            DialogManager.Instance.ShowDialog("Quit");
        }
    }

    public override void Release()
    {
        // 종료 시 처리할 것들
    }
}
