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
        isGameover = true;

        DeleteBoard();
    }


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

#region Tile Movement

    private bool isChanged = false;
    public bool IsChanged { get { return isChanged; } }

    public void MoveTile(Tile _selectTile)
    {
        if(_selectTile == null || isChanged)
        {
            return;
        }

        int selectIndex = _selectTile.index;
        var blankCoord = IndexToCoord(blankIndex);
        var selectCoord = IndexToCoord(selectIndex);

        if (blankCoord.x == selectCoord.x)
        {
            MoveVertical(selectIndex, selectCoord, blankCoord);
        }
        else if(blankCoord.y == selectCoord.y)
        {
            MoveHorizontal(selectIndex, selectCoord, blankCoord);
        }
    }

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

    private Vector2 IndexToCoord(int _index)
    {
        int x = _index == 0 ? 0 : _index % stage.boardSize;
        int y = _index == 0 ? 0 : _index / stage.boardSize;

        return new Vector2(x, y);
    }

    private int CoordToIndex(Vector2 _coord)
    {
        return (int)(_coord.y * stage.boardSize + _coord.x);
    }

    private Vector2 CoordToPosition(Vector2 _coord)
    {
        return new Vector2(startX + tileSize * _coord.x, startY - tileSize * _coord.y);
    }

    #endregion

#region Tile Attack

    public void StartAttack()
    {
        StartCoroutine(Attack());
    }

    public IEnumerator Attack()
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

                    // 0,0 dot 1.0
                    Vector2 a = attackRange[j] - coord;
                    Debug.Log("" + a);
                    int b = (int)(a.x * 10 + a.y);

                    float time = 0f;

                    switch(b)
                    {
                        case 10:
                            tiles[i].animator.Play("Tile_Attack_Right");
                            break;
                        case -10:
                            tiles[i].animator.Play("Tile_Attack_Left");
                            break;
                        case 1:
                            tiles[i].animator.Play("Tile_Attack_Down");
                            break;
                        case -1:
                            tiles[i].animator.Play("Tile_Attack_Up");
                            break;
                    }

                    while (time < tiles[i].animator.GetCurrentAnimatorStateInfo(0).length)
                    {
                        time += Time.deltaTime;
                        yield return null;
                    }
                }
            }
        }
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
}
