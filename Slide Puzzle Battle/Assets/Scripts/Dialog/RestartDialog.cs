using UnityEngine;
using com.PlugStudio.Patterns;

public class RestartDialog : Dialog
{
    public override void PositiveAction()
    {
        gameObject.SetActive(false);
        // 다시하기
        GameManager.Instance.RestartGame();
    }
}