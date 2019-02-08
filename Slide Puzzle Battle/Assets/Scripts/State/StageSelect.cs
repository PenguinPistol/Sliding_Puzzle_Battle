using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.PlugStudio.Patterns;

public class StageSelect : State
{
    public override void Init(params object[] datas)
    {
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
