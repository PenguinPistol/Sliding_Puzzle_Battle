using UnityEngine;

public class FailedDialog : Dialog
{
    public override void Initialized()
    {
    }

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
#if UNITY_EDITOR
        gameObject.SetActive(false);
        GameManager.Instance.ContinueGame();
#elif UNITY_ANDROID
        AdsManager.Instance.ShowRewardContinue(this);
#endif
    }
    
#if UNITY_ANDROID
    private void OnEnable()
    {
        positiveButton.gameObject.SetActive(AdsManager.Instance.LoadedReward);
    }
#endif

    private void Update()
    {
        if(GameManager.Instance.IsContinued)
        {
            positiveButton.interactable = false;
        }
        else
        {
            positiveButton.interactable = true;
        }
    }
}
