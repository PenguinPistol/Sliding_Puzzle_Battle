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
    // 타일 사이즈
    private float tileSize;
    // 시작 위치 X 좌표값
    private float startX;
    // 시작 위치 Y 좌표값
    private float startY;
    // 공격 진행중
    private bool isAttacked;

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

    public int BoardSize { get { return stage.boardSize; } }
    public bool IsAttacked { get { return isAttacked; } }

#region Game Controll

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

            StartCoroutine(CreateBoard(stage.boardSize));
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
        state = PlayState.Ready;
        isGameover = true;

        DeleteBoard();
    }
    #endregion

#region Board Controll

    public IEnumerator CreateBoard(int _boardSize)
    {
        tiles = new List<Tile>();
        tileSize = boardWidth / _boardSize - spacing;
        startX = -(tileSize / 2f) * (_boardSize - 1);
        startY = -startX;

        int tileCount = _boardSize * _boardSize;
        int monsterCount = stage.monsterCount;
        int swordCount = monsterCount * 2;

        for (int i = 0; i < tileCount; i++)
        {
            var type = TileData.TileType.Normal;
            var coord = IndexToCoord(i);
            var pos = CoordToPosition(coord);

            // 가장자리에 칼 / 안쪽에 몬스터
            if(coord.x == 0 || coord.y == 0 || coord.x == _boardSize-1 || coord.y == _boardSize-1 )
            {
                //칼생성
                if(swordCount > 0)
                {
                    if (Random.Range(0f, 2f) < 1f)
                    {
                        type = TileData.TileType.Attack;
                        --swordCount;
                    }
                }
            }
            else 
            {
                if (monsterCount > 0)
                {
                    if (Random.Range(0f, 2f) < 1f)
                    {
                        type = TileData.TileType.Monster;
                        --monsterCount;
                    }
                }
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

            yield return new WaitForSeconds(0.05f);
        }

        blankIndex = tiles.Count - 1;

        StartCoroutine(Game());
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

#endregion

#region Tile Movement

    private bool isChanged = false;
    public bool IsChanged { get { return isChanged; } }
    
    public void MoveTile(int _selectIndex)
    {
        if (isChanged)
        {
            return;
        }

        var blankCoord = IndexToCoord(blankIndex);
        var selectCoord = IndexToCoord(_selectIndex);

        if (blankCoord.x == selectCoord.x)
        {
            MoveVertical(_selectIndex, selectCoord, blankCoord);
        }
        else if (blankCoord.y == selectCoord.y)
        {
            MoveHorizontal(_selectIndex, selectCoord, blankCoord);
        }

        //tiles.Sort(delegate (Tile a, Tile b)
        //{
        //    if (a == null || b == null) return 0;

        //    if (a.index > b.index) return 1;
        //    else if (a.index < b.index) return -1;
        //    else return 0;
        //});
    }

    private void MoveVertical(int _selectIndex, Vector2 _selectCoord, Vector2 _blankCoord)
    {
        isChanged = true;

        int count = (int)Mathf.Abs(_blankCoord.y - _selectCoord.y);

        for (int i = count - 1; i >= 0; i--)
        {
            int index = _selectIndex - stage.boardSize * i;
            int target = _selectIndex - stage.boardSize * (i + 1);

            if (_blankCoord.y > _selectCoord.y)
            {
                index = _selectIndex + stage.boardSize * i;
                target = _selectIndex + stage.boardSize * (i + 1);
            }

            var targetPosition = CoordToPosition(IndexToCoord(target));
            var tile = tiles.Find(item => item.index == index);

            tile.index = target;
            tile.MovePosition(targetPosition);
        }

        blankIndex = _selectIndex;
        isChanged = false;
    }

    private void MoveHorizontal(int _selectIndex, Vector2 _selectCoord, Vector2 _blankCoord)
    {
        isChanged = true;

        int count = (int)Mathf.Abs(_blankCoord.x - _selectCoord.x);

        for (int i = count - 1; i >= 0; i--)
        {
            int index = _selectIndex - i;
            int targetIndex = _selectIndex - (i + 1);

            if (_blankCoord.x > _selectCoord.x)
            {
                index = _selectIndex + i;
                targetIndex = _selectIndex + (i + 1);
            }

            var targetPosition = CoordToPosition(IndexToCoord(targetIndex));
            var tile = tiles.Find(item => item.index == index);

            tile.index = targetIndex;
            tile.MovePosition(targetPosition);
        }

        blankIndex = _selectIndex;
        isChanged = false;
    }

    public Vector2 IndexToCoord(int _index)
    {
        int x = _index == 0 ? 0 : _index % stage.boardSize;
        int y = _index == 0 ? 0 : _index / stage.boardSize;

        return new Vector2(x, y);
    }

    public int CoordToIndex(Vector2 _coord)
    {
        return (int)(_coord.y * stage.boardSize + _coord.x);
    }

    public Vector2 CoordToPosition(Vector2 _coord)
    {
        return new Vector2(startX + tileSize * _coord.x, startY - tileSize * _coord.y);
    }

    #endregion

#region Tile Attack

    public void StartAttack()
    {
        if (isAttacked)
        {
            return;
        }

        StartCoroutine(Attack());
    }

    public IEnumerator Attack()
    {
        isAttacked = true;

        for (int i = 0; i < tiles.Count - 1; i++)
        {
            if(tiles[i].Type.Equals(TileData.TileType.Normal)
                || tiles[i].Type.Equals(TileData.TileType.Monster))
            {
                continue;
            }

            tiles[i].Attack();

            while(tiles[i].IsAttack)
            {
                yield return null;
            }
        }

        isAttacked = false;
    }

#endregion

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


    public List<Tile> GetRangeTiles(int _index)
    {
        var result = new List<Tile>();

        Tile tile = tiles.Find(x => x.index == _index);

        if(tile == null)
        {
            return result;
        }

        var range = tile.AttackRange;

        for (int i = 0; i < range.Length; i++)
        {
            var coord = IndexToCoord(_index) + range[i];

            if (coord.x < 0
                    || coord.x >= stage.boardSize
                    || coord.y < 0
                    || coord.y >= stage.boardSize)
            {
                continue;
            }

            var targetIndex = CoordToIndex(coord);

            try
            {
                Tile findTile = tiles.Find(x => x.index == targetIndex);

                if(findTile.Type.Equals(TileData.TileType.Monster)
                    || findTile.Type.Equals(TileData.TileType.Normal))
                {
                    result.Add(findTile);
                }
            }
            catch (System.NullReferenceException e)
            {
                com.PlugStudio.Utility.DebugX(e.Message);
            }
        }

        return result;
    }
}
