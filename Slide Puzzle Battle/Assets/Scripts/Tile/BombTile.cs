using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTile : WeaponTile
{
    public BombTile(Sprite _sprite, float _size, int _damage, int _puzzleSize)
        : base(_sprite, _size, _damage)
    {
        attackRange = new Vector2[(_puzzleSize - 1) * 4];

        Vector2[] direction
            = { Vector2.right, Vector2.down, Vector2.left, Vector2.up };

        for (int i = 0; i < attackRange.Length; i += 4)
        {
            attackRange[i] = direction[i / 4] * (i / _puzzleSize + 1);
        }
    }

    public BombTile(BombTile _other)
        : base(_other)
    {
    }

    public override IEnumerator Attack(List<Tile> _scopes)
    {
        for (int i = 0; i < _scopes.Count; i++)
        {
            if(_scopes[i] == null)
            {
                continue;
            }

            _scopes[i].controller.animator.Play("Tile_Attack_Bomb");

            if(_scopes[i].GetType().Equals(typeof(MonsterTile)))
            {
                ((MonsterTile)_scopes[i]).Damaged(1);
            }

            float playTime = 0f;
            float length = _scopes[i].controller.animator.GetCurrentAnimatorStateInfo(0).length;

            float delay = (i == _scopes.Count - 1) ? length : length/20f;

            while (playTime < delay)
            {
                playTime += Time.deltaTime;

                // animation event 써서 파티클 생성하기 -> 보류

                yield return null;
            }
        }
    }

    /*
    public BombTile(TileType _type, Sprite _icon, float _rate)
       : base(_type, _icon, _rate)
    {
       rate = 0;

       attackRange = new Vector2[(GameManager.Instance.BoardSize-1) * 4];

       for (int i = 0; i < attackRange.Length; i += 4)
       {
           attackRange[i] = Vector2.right * (i/4 + 1);
           attackRange[i+1] = Vector2.down * (i/4 + 1);
           attackRange[i+2] = Vector2.left * (i/4 + 1);
           attackRange[i+3] = Vector2.up * (i/4 + 1);
       }
    }

    public override void AfterAttack()
    {
    }

    public override void Attack(Vector2 _position, Vector2 _direction)
    {
    }

    public override IEnumerator AttackAnimation(Animator _animator, List<Tile> _range)
    {
       if (isAttack == false)
       {
           isAttack = true;

           for (int i = 0; i < _range.Count; i++)
           {
               if(_range[i].Type.Equals(TileType.Monster) || _range[i].Type.Equals(TileType.Normal))
               {

                   _range[i].animator.Play("Tile_Attack_Bomb");

                   float aniTime = 0f;

                   float aniLength = _animator.GetCurrentAnimatorStateInfo(0).length / 20f;

                   if (i == _range.Count - 1)
                   {
                       aniLength = _animator.GetCurrentAnimatorStateInfo(0).length;
                   }

                   while (aniTime < aniLength)
                   {
                       aniTime += Time.deltaTime;
                       yield return null;
                   }

                   if (_range[i].Type.Equals(TileType.Monster))
                   {
                       // 체력 감소
                   }

                   if (i == _range.Count-1)
                   {
                       isAttack = false;
                   }
               }
           }

       }
    }

    public override void Execute()
    {
    }
    */
}
