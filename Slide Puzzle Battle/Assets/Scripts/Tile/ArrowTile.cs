﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.PlugStudio;

public class ArrowTile : WeaponTile
{
    public ArrowTile(Sprite _sprite, float _size, int _damage)
        : base(_sprite, _size, _damage)
    {
        attackRange = new Vector2[]{
              Vector2.down + Vector2.left
            , Vector2.down + Vector2.right
            , Vector2.up + Vector2.left
            , Vector2.up  + Vector2.right
        };
    }

    public ArrowTile(BombTile _other)
        : base(_other)
    {
    }

    public override IEnumerator Attack(List<Tile> _scopes)
    {
        yield return null;
    }

    /*
    public ArrowTile(TileType _type, Sprite _icon, float _rate)
        : base(_type, _icon, _rate)
    {
        rate = 0;

        attackRange = new Vector2[]{
              Vector2.down + Vector2.left    // 0, 1(실제론 Vector.down
            , Vector2.down + Vector2.right
            , Vector2.up + Vector2.left
            , Vector2.up  + Vector2.right
        };
    }

    public override void AfterAttack()
    {
    }

    public override void Attack(Vector2 _position, Vector2 _direction)
    {
    }

    public override IEnumerator AttackAnimation(Animator _animator, List<Tile> _range)
    {
        Transform transform = _animator.transform;
        Transform background = transform.GetChild(0).transform;

        for (int i = 0; i < _range.Count; i++)
        {
            if(!_range[i].Type.Equals(TileType.Monster))
            {
                continue;
            }

            Utility.LookAt2D(transform, _range[i].transform);

            Vector3 angle = transform.eulerAngles;
            angle.z = -angle.z;

            background.localRotation = Quaternion.Euler(angle);

            if (isAttack == false)
            {
                isAttack = true;

                _animator.Play("Tile_Attack_Up");

                float aniTime = 0f;

                while (aniTime < _animator.GetCurrentAnimatorStateInfo(0).length)
                {
                    aniTime += Time.deltaTime;

                    yield return null;
                }

                isAttack = false;
            }
        }
    }

    public override void Execute()
    {
    }
    */
}
