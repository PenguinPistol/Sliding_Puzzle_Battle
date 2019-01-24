using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.PlugStudio.Patterns;

public class Game : State
{
    public override void Init(params object[] datas)
    {
        DialogData quitGame = new DialogData
            .Builder("진행중인 게임이 저장되지 않습니다.\n종료 하시겠습니까?")
            .SetPositiveText("나가기")
            .SetNegativeText("취소")
            .SetPositiveAction(() =>
            {
                GameManager.Instance.FinishGame();
                StateController.Instance.ChangeBeforeState();
            }).Build();

        DialogManager.Instance.AddDialog(quitGame, "QuitGame");
    }

    public override void Execute()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            DialogManager.Instance.ShowDialog("QuitGame");
        }
    }

    public override void Exit()
    {
    }
}
