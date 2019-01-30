using UnityEngine;
using com.PlugStudio.Patterns;

public class LeaveStageDialog : Dialog
{
    public override void PositiveAction()
    {
        StateController.Instance.ChangeBeforeState();
    }
}