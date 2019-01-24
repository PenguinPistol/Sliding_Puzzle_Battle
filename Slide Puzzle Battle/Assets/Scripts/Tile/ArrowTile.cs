using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTile : TileData
{
    public ArrowTile(Sprite _icon, float _rate)
        : base(_icon, _rate)
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
