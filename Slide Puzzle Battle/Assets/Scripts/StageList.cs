using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.PlugStudio.Patterns;

public class StageList : ListView<StageListItem, StageData>
{
    private void Start()
    {
        List<StageData> items = new List<StageData>();

        for (int i = 0; i < 10; i++)
        {
            int size = Random.Range(0, 5) + 4;

            var data = new StageData(size, 0, 0, 1);
            items.Add(data);
        }

        Init(items);
    }

    public override void Init(List<StageData> _items)
    {
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
        StateController.Instance.ChangeState("Game");
        GameManager.Instance.StartGame(items[_index].Data);
    }
}
