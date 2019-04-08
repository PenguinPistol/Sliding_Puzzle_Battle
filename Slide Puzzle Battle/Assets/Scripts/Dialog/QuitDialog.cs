using UnityEngine;

public class QuitDialog : Dialog
{
    public override void Initialized()
    {
        if (Application.systemLanguage.ToString().Equals("Korean"))
        {
            messageText.text = "게임을 종료하시겠습니까?";
        }
    }

    public override void PositiveAction()
    {
        Application.Quit();
    }
}