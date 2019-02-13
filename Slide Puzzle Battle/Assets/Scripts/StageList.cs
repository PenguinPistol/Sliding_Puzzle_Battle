using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.PlugStudio.Patterns;

public class StageList : ListView<StageListItem, StageData>
{
    public override void Init(List<StageData> _items)
    {
        int completeLevel = Database.Instance.CompleteLastLevel;

        for (int i = 0; i < _items.Count; i++)
        {
            var item = Instantiate(listItemPrefab, contentView);

            var listItem = item.GetComponent<StageListItem>();

            if (listItem == null)
            {
                listItem = item.AddComponent<StageListItem>();
            }
            listItem.Init(i, _items[i], "Stage " + i);

            var click = item.GetComponent<Button>();

            if(click == null)
            {
                click = item.AddComponent<Button>();
            }

            click.onClick.AddListener(() =>
            {
                SelectItem(listItem.Id);
            });

            items.Add(listItem);
        }
    }

    public override void SelectItem(int _index)
    {
        if(items[_index].Data.state.Equals(StageData.StageState.Lock))
        {
            return;
        }

        StateController.Instance.ChangeState("Game", _index);
    }
}
