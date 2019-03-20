using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.PlugStudio.Patterns;

public class TileFactory : Singleton<TileFactory>
{
    private const string SPRITE_NORMAL = "Sprites/Tiles/Tiles_Normal";
    private const string SPRITE_SWORD = "Sprites/Tiles/Tiles_Sword";
    private const string SPRITE_ARROW = "Sprites/Tiles/Tiles_Arrow";
    private const string SPRITE_BOMB = "Sprites/Tiles/Tiles_Bomb";
    private const string SPRITE_MONSTER = "Sprites/Tiles/Tiles_Monster";

    public enum TileType
    {
        Normal, Sword, Arrow, Bomb, Monster
    }

    private float tileSize;

    public void Init(float _tileSize)
    {
        tileSize = _tileSize;
    }

    public Tile Create(TileType _type, int _index, params object[] _data)
    {
        Tile tile = null;
        Sprite sprite = null;

        switch(_type)
        {
            case TileType.Normal:
                sprite = Resources.Load<Sprite>(SPRITE_NORMAL);
                tile = new NormalTile(sprite, tileSize, _index);
                break;

            case TileType.Sword:
                sprite = Resources.Load<Sprite>(SPRITE_SWORD);
                tile = new SwordTile(sprite, tileSize, _index, GameConst.Damage_SwordTile);
                break;

            case TileType.Arrow:
                sprite = Resources.Load<Sprite>(SPRITE_ARROW);
                tile = new ArrowTile(sprite, tileSize, _index, GameConst.Damage_ArrowTile);
                break;

            case TileType.Bomb:
                sprite = Resources.Load<Sprite>(SPRITE_BOMB);
                tile = new BombTile(sprite, tileSize, _index, GameConst.Damage_BombTile, (int)_data[0]);
                break;

            case TileType.Monster:
                sprite = Resources.Load<Sprite>(SPRITE_MONSTER);
                tile = new MonsterTile(sprite, tileSize, _index, (int)_data[0]);
                break;
        }

        return tile;
    }
}