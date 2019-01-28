using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTile : TileData
{
    public ArrowTile(TileType _type, Sprite _icon, float _rate)
        : base(_type, _icon, _rate)
    {
        rate = 0;
    }

    public override void AfterAttack()
    {
    }

    public override void Attack()
    {
    }
}
