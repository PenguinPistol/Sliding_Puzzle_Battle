using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.PlugStudio.Patterns;

public class Game : State
{
    private Ray2D ray;
    private RaycastHit2D hit;

    private StageData stage;

    public override void Init(params object[] datas)
    {
        stage = Database.Instance.StageDatas[(int)datas[0]];

        GameManager.Instance.StartGame(stage);

        InputController.Instance.AddObservable(this);
    }

    public override void Execute()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            DialogManager.Instance.ShowDialog("LeaveStage");
        }
    }

    public override void Exit()
    {
        GameManager.Instance.FinishGame();
    }

    public override void TouchBegan(Vector3 _touchPosition, int _index)
    {
        if(GameManager.Instance.IsChanged)
        {
            return;
        }
    }
}
