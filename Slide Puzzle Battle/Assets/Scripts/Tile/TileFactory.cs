using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.PlugStudio;

public class TileFactory
{
    public enum TileType
    {
        Normal, Monster, Sword, Bomb, Bow
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

        switch(_type)
        {
            case TileType.Monster:
                Sprite icon = Resources.Load<Sprite>("");
                data = new MonsterTile(icon, _rate);
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
