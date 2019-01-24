using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileData
{
    public Sprite icon;
    public float rate;
    public Vector2 position;

    public TileData(Sprite _icon, float _rate)
    {
        icon = _icon;
        rate = _rate;
    }

    public abstract void Attack();
    public abstract void AfterAttack();
}
