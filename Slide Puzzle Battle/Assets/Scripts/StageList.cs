﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.PlugStudio.Patterns;

public class StageList : ListView<StageListItem, StageData>
{
    public override void SelectItem(int _index)
    {
        if(items[_index].Data.state.Equals(StageData.StageState.Lock))
        {
            Debug.Log("stage is lock!!");
            return;
        }

        GameManager.Instance.LoadLevel(_index);
    }

    public void CheckClear()
    {
        int completeLevel = GameManager.Instance.CompleteLevel;

        if (completeLevel == 0)
        {
            return;
        }

        if(completeLevel == GameManager.Instance.Stages.Count - 1)
        {
            for (int i = 0; i < GameManager.Instance.Stages.Count; i++)
            {
                items[i].Data.state = StageData.StageState.Clear;
                items[i].animator.Play("Clear_Idle");
            }
        }
        else
        {
            for (int i = 0; i < completeLevel - 1; i++)
            {
                items[i].Data.state = StageData.StageState.Clear;
                items[i].animator.Play("Clear_Idle");
            }

            items[completeLevel-1].Data.state = StageData.StageState.Clear;
            items[completeLevel-1].animator.Play("Clear");

            items[completeLevel].Data.state = StageData.StageState.Unlock;
            items[completeLevel].animator.Play("Unlock");
        }

    }
}
