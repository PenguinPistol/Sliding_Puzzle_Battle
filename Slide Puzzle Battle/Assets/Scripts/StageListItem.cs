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
}
