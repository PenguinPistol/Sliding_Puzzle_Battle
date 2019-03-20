using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonsterTile : Tile
{
    // 체력
    private int hp;
    private bool isDead;

    public bool IsDead { get { return isDead; } }

    public MonsterTile(Sprite _sprite, float _size, int _index, int _hp)
        : base(_sprite, _size, _index)
    {
        hp = _hp;
        isDead = (hp <= 0);
    }

    public void OnDamaged(int _damage)
    {
        if (isDead)
        {
            return;
        }

        hp = (hp - _damage) <= 0 ? 0 : hp - _damage;

        if(hp == 0)
        {
            //GameManager.Instance.currentMonsterCount -= 1;
            controller.text.text = "";
            isDead = true;
        }
    }

    public override void Execute()
    {
        if(hp > 0)
        {
            controller.text.text = string.Format("{0}", hp);
        }
        else
        {
            controller.text.text = "";
        }
    }
}
