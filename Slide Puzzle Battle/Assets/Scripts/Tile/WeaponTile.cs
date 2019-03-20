using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class WeaponTile : Tile
{
    protected Vector2[] attackRange;
    protected int damage;

    public int Damage { get { return damage; } }
    public Vector2[] AttackRanges { get { return attackRange; } }

    public WeaponTile(Sprite _sprite, float _size, int _index, int _damage)
        : base(_sprite, _size, _index)
    {
        damage = _damage;
    }

    public abstract IEnumerator Attack(List<Tile> _scopes);
}
