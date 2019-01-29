using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private TileData data;
    private bool isMoved;

    public Image    sprite;
    public Text     rate;
    public int      index;

    public TileData.TileType Type
    {
        get { return data.type; }
    }

    public RectTransform rectTransform;

    public void InitData(TileData _data, Vector2 _position, float _size, int _index)
    {
        data = _data;
        sprite = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();


        if (data.icon != null)
        {
            sprite.enabled = true;
            sprite.sprite = data.icon;
        }

        if(data.rate != 0)
        {
            rate.enabled = true;
            rate.text = string.Format("{0}", data.rate);
        }

        rectTransform.sizeDelta = new Vector3(_size, _size);
        StartCoroutine(CreateAnimation(_position));
        //rectTransform.localPosition = _position;

        index = _index;
        isMoved = false;
    }

    public void MovePosition(Vector3 _position)
    {
        if (isMoved == false)
        {
            isMoved = true;
            StartCoroutine(MoveTarget(_position));
        }
    }

    private IEnumerator MoveTarget(Vector3 _target)
    {
        while(Vector3.Distance(rectTransform.localPosition, _target) > 0.1f)
        {
            rectTransform.localPosition = Vector3.MoveTowards(rectTransform.localPosition, _target, 20);
            yield return null;
        }

        rectTransform.localPosition = _target;
        isMoved = false;
    }

    private IEnumerator CreateAnimation(Vector3 _position)
    {
        rectTransform.localPosition = _position + (Vector3.up * 400f);

        while (Vector3.Distance(rectTransform.localPosition, _position) > 0.1f)
        {
            rectTransform.localPosition = Vector3.MoveTowards(rectTransform.localPosition, _position, 100);
            yield return null;
        }

        rectTransform.localPosition = _position;

        GetComponent<Button>().onClick.AddListener(() => {
            Debug.Log("index : " + index);
            GameManager.Instance.MoveTile(index);
        });
    }
}
