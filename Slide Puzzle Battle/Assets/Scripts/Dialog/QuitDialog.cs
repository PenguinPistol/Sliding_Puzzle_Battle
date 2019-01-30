using UnityEngine;

public class QuitDialog : Dialog
{
    public override void PositiveAction()
    {
        Application.Quit();
    }
}