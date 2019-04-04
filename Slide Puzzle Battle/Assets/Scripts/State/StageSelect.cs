using System.Collections;
using UnityEngine;
using com.PlugStudio.Patterns;

public class StageSelect : State
{
    [Header("UI")]
    public RectTransform canvas;
    public RectTransform contentView;
    public RectTransform settingView;
    public StageList listView;

    private static bool isInit = false;
    private static int beforeCompleteLevel = 0;

    public override IEnumerator Initialize(params object[] _data)
    {
        Vector2 size = contentView.sizeDelta;

        size.y = SizeChanger.Calculate(canvas);
        
        contentView.sizeDelta = size;
        settingView.sizeDelta = size;

        if (AdsManager.Instance.LoadedReward == false)
        {
            AdsManager.Instance.RequestReward();
        }

        yield return listView.Init(GameManager.Instance.Stages);
        yield return listView.SetScrollPosition(GameManager.Instance.LastPlayedLevel + 1, true);
        SoundManager.Instance.PlayBGM(0);
    }

    public override void Begin()
    {
        if(isInit == false)
        {
            isInit = true;


            beforeCompleteLevel = GameManager.Instance.CompleteLevel;
        }
        else
        {
            if (beforeCompleteLevel != GameManager.Instance.CompleteLevel)
            {
                beforeCompleteLevel = GameManager.Instance.CompleteLevel;

                StartCoroutine(listView.CheckClear());
            }
        }

        AdsManager.Instance.ShowBanner();
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
