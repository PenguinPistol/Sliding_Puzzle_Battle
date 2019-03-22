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
            var dir = direction[i % 4];
            var num = ((i / 4) + 1);

            attackRange[i] = direction[i % 4] * ((i / 4) + 1);
        }
    }

    public override IEnumerator Attack(List<Tile> _scopes)
    {
        controller.animator.Play("Tile_Attack_Bomb");

        int count = 0;

        for (int i = 0; i < _scopes.Count; i++)
        {
            if(_scopes[i] == null)
            {
                continue;
            }

            _scopes[i].controller.animator.Play("Tile_Attack_Bomb");

            if(_scopes[i].GetType().Equals(typeof(MonsterTile)))
            {
                count++;
                ((MonsterTile)_scopes[i]).OnDamaged(1);
            }

            float playTime = 0f;
            float length = _scopes[i].controller.animator.GetCurrentAnimatorStateInfo(0).length;

            float delay = (i == _scopes.Count - 1) ? length : length/20f;

            while (playTime < delay)
            {
                playTime += Time.deltaTime;
                yield return null;
            }

            if(i == _scopes.Count - 1)
            {
                length = _scopes[i].controller.animator.GetCurrentAnimatorStateInfo(0).length;
                playTime = 0f;

                while (playTime < length)
                {
                    playTime += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }

    public override void Execute()
    {
    }
}
