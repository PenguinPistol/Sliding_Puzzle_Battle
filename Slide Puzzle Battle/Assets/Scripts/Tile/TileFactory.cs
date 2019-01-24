using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.PlugStudio;

public class TileFactory
{
    public enum TileType
    {
        Normal, Monster, Attack, Bomb, Arrow
    }

    private Transform board;
    private GameObject tilePrefab;

    public TileFactory(GameObject _prefab, Transform _board = null)
    {
        board = _board;
        tilePrefab = _prefab;
    }

    public Tile Create(TileType _type, float _rate)
    {
        TileData data = new NormalTile(null, 0);
        Sprite icon = null;

        switch (_type)
        {
            case TileType.Monster:
                icon = Resources.Load<Sprite>("Sprites/Tiles/Tiles_Monster");
                data = new MonsterTile(icon, _rate);
                break;

            case TileType.Attack:
                icon = Resources.Load<Sprite>("Sprites/Tiles/Tiles_Attack");
                data = new AttackTile(icon, _rate);
                break;

            case TileType.Bomb:
                icon = Resources.Load<Sprite>("Sprites/Tiles/Tiles_Boom");
                data = new BoomTile(icon, _rate);
                break;

            case TileType.Arrow:
                icon = Resources.Load<Sprite>("Sprites/Tiles/Tiles_Arrow");
                data = new ArrowTile(icon, _rate);
                break;

        }

        var newTile = Object.Instantiate(tilePrefab, board);
        var tile = newTile.GetComponent<Tile>();

        if(tile == null)
        {
            newTile.AddComponent<Tile>();
        }

        tile.InitData(data);

        return tile;
    }
}
