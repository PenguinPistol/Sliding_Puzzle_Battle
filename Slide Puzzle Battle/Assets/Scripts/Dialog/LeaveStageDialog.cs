using UnityEngine;
using com.PlugStudio.Patterns;

public class LeaveStageDialog : Dialog
{
    public override void PositiveAction()
    {
        gameObject.SetActive(false);
        StateController.Instance.ChangeState("StageSelect", true);
    }
}