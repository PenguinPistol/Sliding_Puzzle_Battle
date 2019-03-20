using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.PlugStudio;

public class SwordTile : WeaponTile
{
    private string[] aniName = {
        "Tile_Attack_Up"
        , "Tile_Attack_Left"
        , "Tile_Attack_Down"
        , "Tile_Attack_Right"
    };
    
    public SwordTile(Sprite _sprite, float _size, int _index, int _damage)
        : base(_sprite, _size, _index, _damage)
    {
        attackRange = new Vector2[]
        {
              Vector2.up
            , Vector2.left
            , Vector2.down
            , Vector2.right
        };
    }

    public override IEnumerator Attack(List<Tile> _scopes)
    {
        for (int i = 0; i < _scopes.Count; i++)
        {
            if(_scopes[i] == null
                || _scopes[i].GetType().Equals(typeof(MonsterTile)) == false)
            {
                continue;
            }

            var dir = _scopes[i].transform.localPosition - transform.localPosition;
            int index = 0;

            for (int j = 0; j < attackRange.Length; j++)
            {
                if(Vector3.Dot(dir.normalized, attackRange[j]) == 1f)
                {
                    index = j;
                    break;
                }
            }

            controller.animator.Play(aniName[index]);

            float playTime = 0f;
            float length = controller.animator.GetCurrentAnimatorStateInfo(0).length;

            while (playTime < length)
            {
                playTime += Time.deltaTime;

                yield return null;
            }

            ((MonsterTile)_scopes[i]).OnDamaged(damage * GameManager.Reinforce);
        }
    }

    public override void Execute()
    {
    }
}
