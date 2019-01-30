using UnityEngine;
using com.PlugStudio.Patterns;

public class RestartDialog : Dialog
{
    public override void PositiveAction()
    {
        // 다시하기
        StateController.Instance.RestartState();
    }
}