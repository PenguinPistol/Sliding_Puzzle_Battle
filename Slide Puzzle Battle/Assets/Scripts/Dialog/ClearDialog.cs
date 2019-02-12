using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.PlugStudio.Patterns;

public class ClearDialog : Dialog
{
    public override void NegativeAction()
    {
        // 스테이지 선택
        gameObject.SetActive(false);
        StateController.Instance.ChangeBeforeState();
    }

    public override void NeatralAction()
    {
        // 다시하기
        gameObject.SetActive(false);
        GameManager.Instance.RestartGame();
    }

    public override void PositiveAction()
    {
        // 다음 스테이지
        gameObject.SetActive(false);
    }
}
