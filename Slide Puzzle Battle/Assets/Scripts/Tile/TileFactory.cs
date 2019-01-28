using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.PlugStudio;

public class TileFactory
{

    private Transform board;
    private GameObject tilePrefab;

    public TileFactory(GameObject _prefab, Transform _board = null)
    {
        board = _board;
        tilePrefab = _prefab;
    }

    public Tile Create(TileData.TileType _type, float _rate, Vector2 _position, float _size, int _index)
    {
        TileData data = null;
        Sprite icon;

        switch (_type)
        {
            case TileData.TileType.Blank:
                icon = Resources.Load<Sprite>("Sprites/Tiles/Tiles_Blank");
                data = new NormalTile(_type, icon, _rate);
                break;

            case TileData.TileType.Normal:
                icon = Resources.Load<Sprite>("Sprites/Tiles/Tiles_Normal");
                data = new NormalTile(_type, icon, _rate);
                break;
            case TileData.TileType.Monster:
                icon = Resources.Load<Sprite>("Sprites/Tiles/Tiles_Monster");
                data = new MonsterTile(_type, icon, _rate);
                break;

            case TileData.TileType.Attack:
                icon = Resources.Load<Sprite>("Sprites/Tiles/Tiles_Attack");
                data = new AttackTile(_type, icon, _rate);
                break;

            case TileData.TileType.Bomb:
                icon = Resources.Load<Sprite>("Sprites/Tiles/Tiles_Boom");
                data = new BoomTile(_type, icon, _rate);
                break;

            case TileData.TileType.Arrow:
                icon = Resources.Load<Sprite>("Sprites/Tiles/Tiles_Arrow");
                data = new ArrowTile(_type, icon, _rate);
                break;

        }

        var newTile = Object.Instantiate(tilePrefab, board);
        var tile = newTile.GetComponent<Tile>();

        if(tile == null)
        {
            newTile.AddComponent<Tile>();
        }

        tile.InitData(data, _position, _size, _index);

        return tile;
    }
}
