using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTile : TileData
{
    public NormalTile(TileType _type, Sprite _icon, float _rate)
        : base(_type, _icon, _rate)
    {
        _icon = null;
        _rate = 0;
    }

    public override void AfterAttack()
    {
    }

    public override void Attack()
    {
    }
}
