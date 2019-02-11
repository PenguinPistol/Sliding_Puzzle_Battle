using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomTile : TileData
{
    public BoomTile(TileType _type, Sprite _icon, float _rate)
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
}
