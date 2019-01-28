using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTile : TileData
{
    public MonsterTile(TileType _type, Sprite _icon, float _rate)
        : base(_type, _icon, _rate)
    {
    }

    public override void Attack()
    {
        // 공격버튼 눌렀을 때 처리
    }

    public override void AfterAttack()
    {
        // 모든 공격 처리 끝난 후
    }
}
