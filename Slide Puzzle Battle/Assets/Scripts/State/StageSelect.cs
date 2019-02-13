using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.PlugStudio.Patterns;

public class StageSelect : State
{
    public StageList listView;

    public override void Init(params object[] datas)
    {
        StartCoroutine(Database.Instance.ReadStages(listView));
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
}
