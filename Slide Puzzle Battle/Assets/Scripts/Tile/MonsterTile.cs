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

    public MonsterTile(Sprite _sprite, float _size, int _hp)
        : base(_sprite, _size)
    {
        hp = _hp;
        isDead = (hp <= 0);
    }

    public MonsterTile(MonsterTile _other)
        : base(_other)
    {
        hp = _other.hp;
        isDead = _other.isDead;
    }

    public void Damaged(int _damage)
    {
        if (isDead)
        {
            return;
        }

        hp = (hp - _damage) <= 0 ? 0 : hp - _damage;

        if(hp == 0)
        {
            GameManager.Instance.currentMonsterCount -= 1;
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
    /*
    private bool isDead;

    public MonsterTile(TileType _type, Sprite _icon, float _rate)
        : base(_type, _icon, _rate)
    {
        isDead = false;
    }

    public override void Attack(Vector2 _position, Vector2 _direction)
    {
        // 공격버튼 눌렀을 때 처리
    }

    public override void AfterAttack()
    {
        // 모든 공격 처리 끝난 후
        // 쏟아지는 빗물은 내 한치앞도 못보게해 몰아치는 바람은 단 한걸음도 못가게해
        // 벼랑끝에 서있는듯이 나 무서워 떨고있지만 작은 두손을 모은 내 기도는 하나뿐이야
        //
    }

    public override void Execute()
    {
        if(isDead == false)
        {
            if(rate <= 0)
            {
                isDead = true;
                GameManager.Instance.currentMonsterCount--;

                Debug.Log("damage!");
            }
        }
    }
    */
}
