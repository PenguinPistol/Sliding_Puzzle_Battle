using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.PlugStudio.Patterns;

public class StageSelect : State
{
    public StageList listView;

    private bool isClear;

    public override void Init(params object[] datas)
    {
        isClear = false;

        if (datas.Length > 0)
        {
            if ((bool)datas[0])
            {
                // 스테이지 언락
                isClear = true;
            }
        }

        StartCoroutine(LoadStageList());
    }

    public override void Execute()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            DialogManager.Instance.ShowDialog("Quit");
        }
    }

    public override void Exit()
    {
    }

    private IEnumerator LoadStageList()
    {
        while (!Database.Instance.StageLoaded)
        {
            yield return null;
        }

        listView.Init(Database.Instance.Stages, isClear);
    }
}
