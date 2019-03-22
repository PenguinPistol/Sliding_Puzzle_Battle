using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.PlugStudio;

public class ArrowTile : WeaponTile
{
    string[] aniName = { "Tile_Attack_DL"
            , "Tile_Attack_DR"
            , "Tile_Attack_UR"
            , "Tile_Attack_UL" };

    public ArrowTile(Sprite _sprite, float _size, int _index, int _damage)
        : base(_sprite, _size, _index, _damage)
    {
        attackRange = new Vector2[]{
              Vector2.down + Vector2.left
            , Vector2.down + Vector2.right
            , Vector2.up + Vector2.right
            , Vector2.up  + Vector2.left
        };
    }

    public override IEnumerator Attack(List<Tile> _scopes)
    {
        for (int i = 0; i < _scopes.Count; i++)
        {
            if (_scopes[i] == null || !_scopes[i].GetType().Equals(typeof(MonsterTile)))
            {
                continue;
            }

            var dir = _scopes[i].transform.localPosition - transform.localPosition;
            int index = 0;

            for (int j = 0; j < attackRange.Length; j++)
            {
                float dotFloor = Mathf.Floor(Vector3.Dot(dir.normalized, attackRange[j]));

                if (dotFloor == 1)
                {
                    index = j;
                    break;
                }
            }

            controller.animator.Play(aniName[index]);

            var length = controller.animator.GetCurrentAnimatorStateInfo(0).length;
            var time = 0f;

            while (time < length)
            {
                time += Time.deltaTime;
                yield return null;
            }

            ((MonsterTile)_scopes[i]).OnDamaged(1);
        }
    }

    public override void Execute()
    {
    }
}
