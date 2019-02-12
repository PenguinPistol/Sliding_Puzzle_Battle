using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileData
{
    public enum TileType
    {
        Normal, Monster, Attack, Bomb, Arrow, Blank
    }

    public Sprite icon;
    public float rate;
    public TileType type;
    public Vector2[] attackRange = new Vector2[0];
    public bool isAttack;

    public TileData(TileType _type, Sprite _icon, float _rate)
    {
        type = _type;
        icon = _icon;
        rate = _rate;
    }

    public abstract void Attack(Vector2 _position, Vector2 _direction);
    public abstract void AfterAttack();

    public virtual IEnumerator AttackAnimation(Animator _animator, List<Tile> _range)
    {
        yield return null;
    }
}
