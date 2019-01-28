using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.PlugStudio.Patterns;

public class GameManager : Singleton<GameManager>
{
    public enum PlayState
    {
        Ready, Play, Pause, Finish
    }

    // 게임오버 체크
    private bool isGameover = true;
    // 플레이 상태
    private PlayState state;

    // 남은 제한시간
    private float currentTimeout;
    // 남은 공격횟수
    private int currentAttackCount;
    // 남아있는 몬스터 수
    private int monsterCount;

    // 현재 스테이지 정보
    private StageData stage;
    // 타일 리스트
    private List<Tile> tiles;

    // 보드 
    public Transform board;
    // 타일 프리팹
    public GameObject tilePrefab;
    // 타일 팩토리
    private TileFactory factory;
    // 타일 간격
    public float spacing;
    // 실제 타일놓는 보드 크기
    public float boardWidth;
    
    public void StartGame(StageData _stage)
    {
        stage = _stage;

        if(state == PlayState.Ready && isGameover)
        {
            state = PlayState.Play;
            isGameover = false;
            factory = new TileFactory(tilePrefab, board);

            //boardSize = Database.Instance....
            // 1000 / boardSize - 20;

            CreateBoard(stage.boardSize);

            StartCoroutine(Game());
        }
    }

    public void PauseGame()
    {
        if(state == PlayState.Pause)
        {
            return;
        }

        state = PlayState.Pause;
    }

    public void ResumeGame()
    {
        if (state != PlayState.Pause)
        {
            return;
        }

        state = PlayState.Play;
    }

    public void FinishGame()
    {
        isGameover = true;

        DeleteBoard();
    }

    private const float BASE_TILE_SIZE = 200f;

    private float tileSize;
    private float startX;
    private float startY;

    public void CreateBoard(int _boardSize)
    {
        tiles = new List<Tile>();
        tileSize = (boardWidth / _boardSize - spacing) / BASE_TILE_SIZE;
        startX = -tileSize * (_boardSize - 1);
        startY = tileSize * (_boardSize - 1);

        int tileCount = _boardSize * _boardSize;

        for (int i = 0; i < tileCount; i++)
        {
            var types = System.Enum.GetValues(typeof(TileData.TileType));
            var type = (TileData.TileType)Random.Range(0, types.Length - 1);
            var coord = IndexToCoord(i);
            var pos = CoordToPosition(coord);

            if(coord.x == 0 || coord.y == 0 || coord.x == _boardSize-1 || coord.y == _boardSize-1 )
            {
                //칼생성
                
            }
            else
            {

            }

            //tiles.Add(factory.Create(type, 0, pos, tileSize, i));

            if (i == tileCount - 1)
            {
                tiles.Add(null);
            }
            else
            {
                //Tile tile = factory.Create(type, 0, pos, tileSize, i);
                tiles.Add(factory.Create(type, 0, pos, tileSize, i));
            }

        }
        blankIndex = tiles.Count - 1;
    }

    ///////////////////////////////////////////////////////////////
    private int blankIndex;
    ///////////////////////////////////////////////////////////////

    public void DeleteBoard()
    {
        int count = board.transform.childCount;

        for (int i = 0; i < count; i++)
        {
            Destroy(board.transform.GetChild(i).gameObject);
        }

        tiles.Clear();
    }

    private bool isChanged = false;
    public bool IsChanged { get { return isChanged; } }

    public void ChangeTile(Tile _selectTile)
    {
        if(_selectTile == null || isChanged)
        {
            return;
        }

        int selectIndex = _selectTile.index;
        var blankCoord = IndexToCoord(blankIndex);
        var selectCoord = IndexToCoord(selectIndex);

        if(blankCoord.x == selectCoord.x)
        {
            isChanged = true;
            // 세로 이동

            int count = (int)Mathf.Abs(blankCoord.y - selectCoord.y);

            for (int i = count - 1; i >= 0; i--)
            {
                int index = selectIndex - stage.boardSize * i;
                int target = selectIndex - stage.boardSize * (i + 1);

                if (blankCoord.y > selectCoord.y)
                {
                    index = selectIndex + stage.boardSize * i;
                    target = selectIndex + stage.boardSize * (i + 1);
                }

                var targetPosition = CoordToPosition(IndexToCoord(target));
                var tile = tiles.Find(item => item.index == index);

                tile.index = target;
                tile.MovePosition(targetPosition);
            }

            blankIndex = selectIndex;
            isChanged = false;
        }
        else if(blankCoord.y == selectCoord.y)
        {
            // 가로이동
            isChanged = true;

            int count = (int)Mathf.Abs(blankCoord.x - selectCoord.x);

            for (int i = count - 1; i >= 0; i--)
            {
                int index = selectIndex - i;
                int target = selectIndex - (i + 1);

                if (blankCoord.x > selectCoord.x)
                {
                    index = selectIndex + i;
                    target = selectIndex + (i + 1);
                }

                var targetPosition = CoordToPosition(IndexToCoord(target));
                var tile = tiles.Find(item => item.index == index);

                tile.index = target;
                tile.MovePosition(targetPosition);
            }

            blankIndex = selectIndex;
            isChanged = false;
        }
    }

    private Vector2 IndexToCoord(int _index)
    {
        int x = _index == 0 ? 0 : _index % stage.boardSize;
        int y = _index == 0 ? 0 : _index / stage.boardSize;

        return new Vector2(x, y);
    }

    public void Attack()
    {
        for (int i = 0; i < tiles.Count - 1; i++)
        {
            if(tiles[i].Type.Equals(TileData.TileType.Normal) || tiles[i].Type.Equals(TileData.TileType.Monster))
            {
                continue;
            }

            var coord = IndexToCoord(tiles[i].index);

            Vector2[] attackRange = null;

            switch(tiles[i].Type)
            {
                case TileData.TileType.Attack:
                    attackRange = new Vector2[]{
                        new Vector2(coord.x-1, coord.y)
                        , new Vector2(coord.x+1, coord.y)
                        , new Vector2(coord.x, coord.y-1)
                        , new Vector2(coord.x, coord.y+1)
                    };
                    break;
                case TileData.TileType.Arrow:
                    attackRange = new Vector2[0];
                    break;
                case TileData.TileType.Bomb:
                    attackRange = new Vector2[0];
                    break;
            }

            for (int j = 0; j < attackRange.Length; j++)
            {
                if(attackRange[j].x < 0
                    || attackRange[j].x >= stage.boardSize
                    || attackRange[j].y < 0
                    || attackRange[j].y >= stage.boardSize)
                {
                    continue;
                }

                int targetIndex = CoordToIndex(attackRange[j]);

                // 공격할 타일이 빈타일이면 넘어감
                if(targetIndex == blankIndex)
                {
                    continue;
                }

                var tile = tiles.Find(x => x.index == targetIndex);

                if(tile.Type.Equals(TileData.TileType.Monster))
                {
                    Debug.LogFormat("{0} 타일이 {1} 타일 공격", coord, attackRange[j]);
                }
            }
        }
    }

    private int CoordToIndex(Vector2 _coord)
    {
        return (int)(_coord.y * stage.boardSize + _coord.x);
    }

    private Vector2 CoordToPosition(Vector2 _coord)
    {
        return new Vector2(startX + tileSize * _coord.x * 2, startY - tileSize * _coord.y * 2);
    }


    private IEnumerator Game()
    {
        while(!isGameover)
        {
            // 시간제한
            // 시간제한이 0이면 시간제한 없음
            if(stage.timeout > 0)
            {
                // 현재제한시간이 0이면 조건 종료
                if(currentTimeout < 0)
                {
                    // 종료 체크
                }
                else
                {
                    currentTimeout = currentTimeout - Time.deltaTime;
                }

            }

            // 횟수제한이 0이면 횟수제한 없음
            if(stage.attackCount > 0)
            {
                // 현재 횟수제한이 0이면 조건 종료

            }


            // 몬스터 수가 0이면 성공


            // 시간제한 및 횟수제한이 0이면 실패

            yield return null;
        }

        state = PlayState.Ready;
    }
}
