using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageListItem : MonoBehaviour
{
    private StageData data;
    private int index;

    public Image icon;
    public Text title;

    public Sprite[] icons;

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

        if(icon != null)
        {
            icon.sprite = icons[(int)data.state];
        }
        title.text = _title;
    }
}
