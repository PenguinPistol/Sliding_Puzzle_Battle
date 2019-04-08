using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageListItem : ListViewItem<StageData>
{
    public Animator animator;
    public List<Text> titleTexts;

    private bool isPlayAnimation;

    public override void Init(StageData _data, int _index)
    {
        data = _data;
        index = _index;
        isPlayAnimation = false;

        int completeLevel = GameManager.Instance.CompleteLevel - 1;

        switch (data.state)
        {
            case StageData.StageState.Lock:
                if (completeLevel > index)
                {
                    data.state = StageData.StageState.Clear;
                    StartCoroutine(PlayAnimation("Clear_Idle"));
                }
                else if (completeLevel == index)
                {
                    data.state = StageData.StageState.Unlock;
                    StartCoroutine(PlayAnimation("Unlock_Idle"));
                }
                break;
            case StageData.StageState.Unlock:
                if (completeLevel > index)
                {
                    data.state = StageData.StageState.Clear;
                    StartCoroutine(PlayAnimation("Clear_Idle"));
                }
                else
                {
                    StartCoroutine(PlayAnimation("Unlock_Idle"));
                }
                break;
            case StageData.StageState.Clear:
                StartCoroutine(PlayAnimation("Clear_Idle"));
                break;
        }

        for (int i = 0; i < titleTexts.Count; i++)
        {
            titleTexts[i].text = data.title;
        }
    }

    public IEnumerator PlayAnimation(string _clipName)
    {
        if(isPlayAnimation == false)
        {
            isPlayAnimation = true;

            animator.Play(_clipName);

            while (!animator.GetCurrentAnimatorStateInfo(0).IsName(_clipName))
            {
                //전환 중일 때 실행되는 부분
                yield return null;
            }

            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                yield return null;
            }

            isPlayAnimation = false;
        }
    }
}
