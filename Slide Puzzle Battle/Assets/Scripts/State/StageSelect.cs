using System.Collections;
using UnityEngine;
using com.PlugStudio.Patterns;

public class StageSelect : State
{
    public StageList listView;

    private static bool isInit = false;
    private static int beforeCompleteLevel = 0;

    public override IEnumerator Initialize(params object[] _data)
    {
        yield return StartCoroutine(listView.Init(GameManager.Instance.Stages));
        SoundManager.Instance.PlayBGM(0);
    }

    public override void FirstFrame()
    {
        if(isInit == false)
        {
            isInit = true;

            AdsManager.Instance.ShowBanner();

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
