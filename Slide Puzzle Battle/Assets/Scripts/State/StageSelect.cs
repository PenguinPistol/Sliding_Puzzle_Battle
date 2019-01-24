using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.PlugStudio.Patterns;

public class StageSelect : State
{
    public override void Init(params object[] datas)
    {
        DialogData testDialog = new DialogData
            .Builder("게임을 종료하시겠습니까?")
            .SetPositiveText("종료")
            .SetNegativeText("취소")
            .SetPositiveAction(() => {
                Application.Quit();
            })
            .Build();

        DialogManager.Instance.AddDialog(testDialog, "Quit");
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
