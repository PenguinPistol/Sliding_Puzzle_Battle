using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.PlugStudio.Patterns;

public class FailedDialog : Dialog
{
    public override void NegativeAction()
    {
        // 스테이지 선택
        gameObject.SetActive(false);

        GameManager.Instance.LeaveLevel();
    }

    public override void NeatralAction()
    {
        // 다시하기
        gameObject.SetActive(false);
        GameManager.Instance.RestartLevel();
    }

    public override void PositiveAction()
    {
        // 광고보고 이어하기
        //gameObject.SetActive(false);
    }
}
