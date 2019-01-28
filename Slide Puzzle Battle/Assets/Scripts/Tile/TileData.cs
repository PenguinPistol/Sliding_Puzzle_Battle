using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileData
{
    public enum TileType
    {
        Normal, Monster, Attack, Bomb, Arrow, Blank
    }

    public Sprite icon;
    public float rate;
    public TileType type;

    public TileData(TileType _type, Sprite _icon, float _rate)
    {
        type = _type;
        icon = _icon;
        rate = _rate;
    }

    public abstract void Attack();
    public abstract void AfterAttack();
}
