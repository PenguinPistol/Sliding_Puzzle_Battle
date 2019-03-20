using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTile : WeaponTile
{
    public BombTile(Sprite _sprite, float _size, int _index, int _damage, int _boardSize)
        : base(_sprite, _size, _index, _damage)
    {
        attackRange = new Vector2[(_boardSize - 1) * 4];

        Vector2[] direction
            = { Vector2.right, Vector2.down, Vector2.left, Vector2.up };

        for (int i = 0; i < attackRange.Length; i++)
        {
            attackRange[i] = direction[i % 4] * ((i / _boardSize) + 1);
        }
    }

    public override IEnumerator Attack(List<Tile> _scopes)
    {
        controller.animator.Play("Tile_Attack_Bomb");

        for (int i = 0; i < _scopes.Count; i++)
        {
            if(_scopes[i] == null)
            {
                continue;
            }

            _scopes[i].controller.animator.Play("Tile_Attack_Bomb");

            if(_scopes[i].GetType().Equals(typeof(MonsterTile)))
            {
                ((MonsterTile)_scopes[i]).OnDamaged(1);
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

    public override void Execute()
    {
    }
}
