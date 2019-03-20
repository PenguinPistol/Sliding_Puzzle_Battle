using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Tile
{
    public int index;
    public Sprite sprite;
    public float size;
    public TileController controller;
    public Transform transform { get { return controller.transform; } }

    public Tile(Sprite _sprite, float _size, int _index)
    {
        sprite = _sprite;
        size = _size;
        index = _index;
    }

    /// <summary>
    /// TileController.Update() 에서 호출
    /// </summary>
    public abstract void Execute();
}
