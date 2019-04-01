using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.PlugStudio.Patterns;

public class ClearDialog : Dialog
{
    public override void Initialized()
    {
    }

    public override void NegativeAction()
    {
        // 스테이지 선택
        gameObject.SetActive(false);

        // 스테이지 클리어
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
        // 다음 스테이지
        gameObject.SetActive(false);

        GameManager.Instance.PlayNextLevel();
    }
}
