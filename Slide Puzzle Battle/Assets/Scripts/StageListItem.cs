using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageListItem : MonoBehaviour
{
    private StageData data;
    private int index;
    private Animator animator;

    public List<Text> titleTexts;
    
    public StageData Data
    {
        get { return data; }
    }
    public int Id
    {
        get { return index; }
    }

    public void Init(int _index, StageData _data, string _title)
    {
        index = _index;
        data = _data;
        animator = GetComponent<Animator>();

        switch(data.state)
        {
            case StageData.StageState.Lock:
                break;
            case StageData.StageState.Unlock:
                animator.Play("ListItem_Unlock_Idle");
                break;
            case StageData.StageState.Clear:
                animator.Play("ListItem_Clear_Idle");
                break;
        }

        for (int i = 0; i < titleTexts.Count; i++)
        {
            titleTexts[i].text = _title;
        }
    }

    // 리스트 로드시 상태변경용
    public void SetState(StageData.StageState _state)
    {
        switch (_state)
        {
            case StageData.StageState.Lock:
                break;
            case StageData.StageState.Unlock:
                animator.Play("ListItem_Unlock_Idle");
                break;
            case StageData.StageState.Clear:
                animator.Play("ListItem_Clear_Idle");
                break;
        }

        data.state = _state;
    }

    public void UnlockLevel()
    {
        data.state = StageData.StageState.Unlock;

        animator.Play("ListItem_Unlock");
    }

    public void CompleteLevel()
    {
        data.state = StageData.StageState.Clear;

        animator.Play("ListItem_Clear");
    }
}
