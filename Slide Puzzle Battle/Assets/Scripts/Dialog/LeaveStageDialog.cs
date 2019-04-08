using UnityEngine;
using com.PlugStudio.Patterns;

public class LeaveStageDialog : Dialog
{
    public override void Initialized()
    {
        if (Application.systemLanguage.ToString().Equals("Korean"))
        {
            messageText.text = "진행 중인 퍼즐의 정보를 저장되지 않습니다.";
        }
    }

    public override void PositiveAction()
    {
        gameObject.SetActive(false);
        GameManager.Instance.LeaveLevel();
    }
}