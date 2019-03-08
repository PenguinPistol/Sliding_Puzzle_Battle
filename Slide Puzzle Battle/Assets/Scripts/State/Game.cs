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

    public Text title;
    public Text attckLimitText; // 공격횟수
    public Text timeLimitText;
    //public Text energyText; // 재화 텍스트 UI

    public override void Init(params object[] datas)
    {
        stage = Database.Instance.Stages[(int)datas[0]];

        title.text = "STAGE " + stage.level;

        if (stage.AttackLimit > 0)
        {
            attckLimitText.gameObject.SetActive(true);
            attckLimitText.text = "" + stage.AttackLimit;
        }

        GameManager.Instance.StartGame(stage);

        InputController.Instance.AddObservable(this);

        if (!stage.isAchieve[0])
        {
            attckLimitText.gameObject.SetActive(false);
        }
        else
        {
            attckLimitText.gameObject.SetActive(true);
        }

        if (!stage.isAchieve[1])
        {
            timeLimitText.gameObject.SetActive(false);
        }
        else
        {
            timeLimitText.gameObject.SetActive(true);
        }
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

        attckLimitText.text = "" + GameManager.Instance.AttackLimit;
        timeLimitText.text = "" + (int)GameManager.Instance.TimeLimit;
    }

    public override void Exit()
    {
        GameManager.Instance.FinishGame();
    }

    private readonly int layerMask = 1 << 8;

    public override void TouchBegan(Vector3 _touchPosition, int _index)
    {
        if(!GameManager.Instance.State.Equals(GameManager.PlayState.Play))
        {
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(_touchPosition, Vector2.zero, 0f, layerMask);

        if (hit.collider != null)
        {
            hit.collider.SendMessage("SelectTile");
        }
    }
}
