using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private TileData data;

    public Image icon;
    public Text rate;

    public void InitData(TileData _data)
    {
        data = _data;

        if(data.icon != null)
        {
            icon.enabled = true;
            icon.sprite = data.icon;
        }

        if(data.rate != 0)
        {
            rate.enabled = true;
            rate.text = data.rate.ToString();
        }

    }
}
