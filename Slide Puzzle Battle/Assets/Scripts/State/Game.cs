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

    public Text attckLimitText; // 공격횟수
    //public Text energyText; // 재화 텍스트 UI

    public Animator settingView;

    public override void Init(params object[] datas)
    {
        stage = Database.Instance.Stages[(int)datas[0]];

        if(stage.AttackLimit > 0)
        {
            attckLimitText.gameObject.SetActive(true);
            attckLimitText.text = "" + stage.AttackLimit;
        }

        GameManager.Instance.StartGame(stage);
        GameManager.Instance.settingView = settingView;

        InputController.Instance.AddObservable(this);
    }

    public override void Execute()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //DialogManager.Instance.ShowDialog("LeaveStage");
            // 게임세팅 보기 & 게임 퍼즈

            GameManager.Instance.PauseGame();
        }

        if(stage == null)
        {
            return;
        }

        if (stage.AttackLimit > 0)
        {
            attckLimitText.text = "" + GameManager.Instance.AttackLimit;
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
