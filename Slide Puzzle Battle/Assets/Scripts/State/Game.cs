using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using com.PlugStudio.Patterns;
using com.PlugStudio.Input;

public class Game : State
{
    private Ray2D ray;
    private RaycastHit2D hit;

    public override void Init(params object[] datas)
    {
        int index = (int)datas[0];

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

        GameManager.Instance.StartGame(Database.Instance.StageData[index]);

        InputController.Instance.AddObservable(this);
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
        GameManager.Instance.DeleteBoard();
    }

    public override void TouchBegan(Vector3 _touchPosition, int _index)
    {
        if(GameManager.Instance.IsChanged)
        {
            return;
        }
    }
}
