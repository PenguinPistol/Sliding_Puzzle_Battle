using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Puzzle
{
    private const float TILE_CREATE_DELAY = 0.05f;
    private const float TILE_SPACING = 0f;
    private const float BOARD_WIDTH = 1000f;

    public enum Type
    {
        Normal, Time, Count, Both
    }

    private Type type;
    private int boardSize;
    private List<Tile> tiles;
    private StageData stage;
    private Transform board;

    private Vector2 startPos;
    private float tileSize;
    private int blankIndex;

    public Type PuzzleType { get { return type; } }
    public int Size { get { return boardSize; } }

    private TileController tileControllerPrefab;

    public Puzzle(StageData _stage, Transform _board)
    {
        stage = _stage;
        boardSize = stage.BoardSize;
        board = _board;
    }

    public void Clean()
    {
        //전부 지우기

        for (int i = 0; i < board.childCount; i++)
        {
            Object.Destroy(board.GetChild(i).gameObject);
        }

        tiles.Clear();
    }

    public IEnumerator CreatePuzzle(TileController _tileController)
    {
        tiles = new List<Tile>();
        tileSize = (BOARD_WIDTH / stage.BoardSize) / 200f;
        startPos = new Vector2
        (
             -tileSize * (stage.BoardSize - 1)
            , tileSize * (stage.BoardSize - 1)
        );
        tileControllerPrefab = _tileController;
        // 1000 / 

        CreateTiles();
        Shuffle();

        for (int i = 0; i < tiles.Count; i++)
        {
            var position = CoordToPosition(IndexToCoord(i));
            var scale = new Vector3(tileSize, tileSize, 1f);

            var newTile = Object.Instantiate(_tileController, board);
            newTile.SetData(tiles[i], position, scale);

            yield return new WaitForSeconds(TILE_CREATE_DELAY);
        }

        // 빈타일 생성
        blankIndex = tiles.Count;
        tiles.Add(null);


        GameManager.Instance.StartCoroutine(GameManager.Instance.Game());
    }

    private void CreateTiles()
    {
        int allTileCount = (stage.BoardSize * stage.BoardSize) - 1;
        int monsterCount = stage.MonsterCount;
        int swordCount = monsterCount * 2;

        for (int i = 0; i < allTileCount; i++)
        {
            Sprite sprite = null;
            Tile tile = null;

            if (i < monsterCount)
            {
                // 몬스터 타일 생성
                sprite = Resources.Load<Sprite>("Sprites/Tiles/Tiles_Monster");
                tile = new MonsterTile(sprite, tileSize, stage.monsters[i]);
            }
            else if (i - monsterCount < swordCount)
            {
                // 칼 타일 생성
                sprite = Resources.Load<Sprite>("Sprites/Tiles/Tiles_Sword");
                tile = new SwordTile(sprite, tileSize, 1);
            }
            else
            {
                // 일반 타일 생성
                sprite = Resources.Load<Sprite>("Sprites/Tiles/Tiles_Normal");
                tile = new Tile(sprite, tileSize);
            }

            if (tile != null)
            {
                tile.index = i;
                tiles.Add(tile);
            }
        }
    }

    public void Relocation()
    {
        var blankTile = tiles.Find(x => x == null);
        tiles.Remove(blankTile);

        // 재배치
        Shuffle();

        for (int i = 0; i < tiles.Count; i++)
        {
            if(tiles[i].GetType().Equals(typeof(MonsterTile)))
            {
                if (((MonsterTile)tiles[i]).IsDead)
                {
                    var controller = tiles[i].controller;
                    var sprite = Resources.Load<Sprite>("Sprites/Tiles/Tiles_Normal");
                    tiles[i] = new Tile(sprite, tileSize);
                    tiles[i].controller = controller;
                }
            }

            var position = CoordToPosition(IndexToCoord(i));
            var scale = new Vector3(tileSize, tileSize, 1f);

            tiles[i].controller.SetData(tiles[i], position, scale);
        }

        blankIndex = tiles.Count;
        tiles.Add(null);
    }

    private void Shuffle()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            int index1 = i;
            int index2 = Random.Range(0, tiles.Count);
            var coord1 = IndexToCoord(index1);
            var coord2 = IndexToCoord(index2);

            bool isEdge1 =
                (coord1.x == 0 || coord1.y == 0  || coord1.x == boardSize - 1 || coord1.y == boardSize - 1);

            bool isEdge2 =
                (coord2.x == 0 || coord2.y == 0 || coord2.x == boardSize - 1 || coord2.y == boardSize - 1);

            if ((tiles[index1].GetType().Equals(typeof(SwordTile)) && !isEdge2)
                || (tiles[index2].GetType().Equals(typeof(SwordTile)) && !isEdge1)
                || (tiles[index1].GetType().Equals(typeof(MonsterTile)) && isEdge2)
                || (tiles[index2].GetType().Equals(typeof(MonsterTile)) && isEdge1))
            {
                // 다시 선택
                i--;
                continue;
            }

            var temp = tiles[index1];

            tiles[index1] = tiles[index2];
            tiles[index1].index = index1;

            tiles[index2] = temp;
            tiles[index2].index = index2;
        }
    }

    public void MoveTile(int _selectIndex)
    {
        int sx = _selectIndex % boardSize;
        int sy = _selectIndex / boardSize;

        int bx = blankIndex % boardSize;
        int by = blankIndex / boardSize;

        if (sx == bx)
        {
            for (int i = Mathf.Abs(by - sy) - 1; i >= 0; i--)
            {
                Vector3 dir = Vector3.up;
                int index = _selectIndex - boardSize * i;
                int change = _selectIndex - boardSize * (i + 1);

                if (sy < by)
                {
                    index = _selectIndex + boardSize * i;
                    dir = Vector3.down;
                    change = _selectIndex + boardSize * (i + 1);
                }

                var tile = tiles.Find(x => x.index == index);

                tile.controller.Move(dir);
                tile.index = change;
            }
        }
        else if (sy == by)
        {
            for (int i = Mathf.Abs(bx - sx) - 1; i >= 0; i--)
            {
                Vector3 dir = Vector3.left;
                int index = _selectIndex - i;
                int change = _selectIndex - (i + 1);

                if (sx < bx)
                {
                    dir = Vector3.right;
                    index = _selectIndex + i;
                    change = _selectIndex + (i + 1);
                }

                var tile = tiles.Find(x => x.index == index);

                tile.controller.Move(dir);
                tile.index = change;
            }
        }
        else
        {
            return;
        }

        blankIndex = _selectIndex;
    }

    public List<WeaponTile> GetWeaponTiles()
    {
        List<WeaponTile> weapons = new List<WeaponTile>();

        for (int i = 0; i < tiles.Count - 1; i++)
        {
            if (tiles[i].GetType().IsSubclassOf(typeof(WeaponTile)))
            {
                weapons.Add((WeaponTile)tiles[i]);
            }
        }

        return weapons;
    }

    public List<T> GetTiles<T>()
        where T : Tile
    {
        List<T> find = new List<T>();

        for (int i = 0; i < tiles.Count - 1; i++)
        {
            if (tiles[i].GetType().Equals(typeof(T)))
            {
                find.Add((T)tiles[i]);
            }
        }

        return find;
    }

    public bool isAttacked;

    public IEnumerator Attack()
    {
        isAttacked = true;

        var weapons = GetWeaponTiles();
        
        foreach (var weapon in weapons)
        {
            List<Tile> scopes = new List<Tile>();

            foreach (var range in weapon.AttackRanges)
            {
                Vector2 targetCoord = IndexToCoord(weapon.index) + range;
                int targetIndex = CoordToIndex(targetCoord);

                scopes.Add(tiles.Find(x => x != null && x.index == targetIndex));
            }

            if(GameManager.Instance.State != GameManager.PlayState.Play)
            {
                break;
            }

            Debug.Log("attack start");
            yield return weapon.controller.StartCoroutine(weapon.Attack(scopes));
            Debug.Log("attack finished");
        }

        Debug.Log("Relocation");
        Relocation();

        isAttacked = false;
    }

    public void ChangeTile(System.Type _type)
    {
        var tiles = GetTiles<Tile>();
        int index = Random.Range(0, tiles.Count);

        WeaponTile newTile = null;
        Sprite sprite = null;

        if(_type.Equals(typeof(SwordTile)))
        {
            sprite = Resources.Load<Sprite>("Sprites/Tiles/Tiles_Sword");
            newTile = new SwordTile(sprite, tileSize, 1);
        }
        else if (_type.Equals(typeof(ArrowTile)))
        {
            sprite = Resources.Load<Sprite>("Sprites/Tiles/Tiles_Arrow");
            newTile = new ArrowTile(sprite, tileSize, 1);
        }
        else if (_type.Equals(typeof(BombTile)))
        {
            sprite = Resources.Load<Sprite>("Sprites/Tiles/Tiles_Bomb");
            newTile = new BombTile(sprite, tileSize, 1, boardSize);
        }
        newTile.index = tiles[index].index;

        var position = CoordToPosition(IndexToCoord(newTile.index));
        var scale = new Vector3(tileSize, tileSize, 1f);

        tiles[index].controller.SetData(newTile, position, scale);

        int newTileIndex = this.tiles.Find(x => x.index == tiles[index].index).index;
        this.tiles[newTileIndex] = newTile;
    }

    public Vector2 IndexToCoord(int _index)
    {
        int x = _index == 0 ? 0 : _index % boardSize;
        int y = _index == 0 ? 0 : _index / boardSize;

        return new Vector2(x, y);
    }

    public int CoordToIndex(Vector2 _coord)
    {
        return (int)(_coord.y * stage.BoardSize + _coord.x);
    }

    public Vector2 CoordToPosition(Vector2 _coord)
    {
        return new Vector2(startPos.x + (tileSize * 2f) * _coord.x, startPos.y - (tileSize * 2f) * _coord.y);
    }
}
