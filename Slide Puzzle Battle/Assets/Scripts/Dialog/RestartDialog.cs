using UnityEngine;
using com.PlugStudio.Patterns;

public class RestartDialog : Dialog
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
        // 다시하기
        gameObject.SetActive(false);
        GameManager.Instance.RestartLevel();
    }
}