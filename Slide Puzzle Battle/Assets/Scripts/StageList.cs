using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.PlugStudio.Patterns;

public class StageList : ListView<StageListItem, StageData>
{
    public override void Init(List<StageData> _items)
    {
        if (items.Count != _items.Count)
        {
            StartCoroutine(InitView(_items, false));
        }
        else
        {
            SetState();
        }
    }

    public void Init(List<StageData> _items, bool _isClear)
    {
        StartCoroutine(InitView(_items, _isClear));
    }

    private void UnlockNextLevel()
    {
        for (int i = 0; i < GameManager.Instance.completeLevel - 1; i++)
        {
            items[i].SetState(StageData.StageState.Clear);
        }

        items[GameManager.Instance.completeLevel - 1].CompleteLevel();

        items[GameManager.Instance.completeLevel].UnlockLevel();

        GameManager.Instance.completeLevel++;
    }

    public override void SelectItem(int _index)
    {
        if(items[_index].Data.state.Equals(StageData.StageState.Lock))
        {
            return;
        }

        StateController.Instance.ChangeState("Game", _index);
    }

    private IEnumerator InitView(List<StageData> _items, bool _isClear)
    {
        float time = Time.time;

        // 리스트 초기화
        for (int i = 0; i < items.Count; i++)
        {
            Destroy(items[i].gameObject);
            yield return null;
        }
        items.Clear();

        time = Time.time - time;

        time = Time.time;

        for (int i = 0; i < _items.Count; i++)
        {
            var item = Instantiate(listItemPrefab, contentView);

            var listItem = item.GetComponent<StageListItem>();

            if (listItem == null)
            {
                listItem = item.AddComponent<StageListItem>();
            }
            listItem.Init(i, _items[i], "Stage " + (i+1));

            var click = item.GetComponent<Button>();

            if (click == null)
            {
                click = item.AddComponent<Button>();
            }

            click.onClick.AddListener(() =>
            {
                SelectItem(listItem.Id);
            });

            items.Add(listItem);

            yield return null;
        }

        time = Time.time - time;

        //Debug.Log("걸린시간 : " + time);

        if(_isClear)
        {
            UnlockNextLevel();
        }
    }

    private void SetState()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetState(items[i].Data.state);
        }
    }
}
