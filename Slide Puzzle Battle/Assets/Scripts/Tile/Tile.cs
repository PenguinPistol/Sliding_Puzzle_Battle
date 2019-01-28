using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    private TileData data;
    private bool isMoved;

    public SpriteRenderer sprite;
    public TextMeshPro rate;
    public int index;

    public TileData.TileType Type
    {
        get { return data.type; }
    }

    public void InitData(TileData _data, Vector2 _position, float _size, int _index)
    {
        data = _data;
        sprite = GetComponent<SpriteRenderer>();

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

        transform.localScale = new Vector3(_size, _size);
        transform.localPosition = _position;

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
        while(Vector3.Distance(transform.position, _target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * 20);
            yield return null;
        }

        transform.position = _target;
        isMoved = false;
    }
}
