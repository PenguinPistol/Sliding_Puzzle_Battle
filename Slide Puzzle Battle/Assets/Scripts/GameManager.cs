using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.PlugStudio.Patterns;

public class GameManager : Singleton<GameManager>
{
    public enum PlayState
    {
        Ready, Play, Pause, Clear, Failed, Finish
    }

    // 게임오버 체크
    private bool isGameover = true;
    // 플레이 상태
    private PlayState state;

    // 남은 공격횟수
    private int currentAttackCount;
    // 남아있는 몬스터 수
    public int currentMonsterCount;

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
    // 빈칸 인덱스
    private int blankIndex;
    // 현재 스테이지 몬스터 체력들
    private int[] monsters;

    // 보드 
    public Transform board;
    // 타일 프리팹
    public TileController tilePrefab;
    // 타일 팩토리
    private TileFactory factory;
    // 타일 간격
    public float spacing;
    // 실제 타일놓는 보드 크기
    public float boardWidth;
    // 
    public Animator settingView;

    public int BoardSize { get { return stage.BoardSize; } }
    public bool IsAttacked { get { return isAttacked; } }
    public int AttackLimit { get { return currentAttackCount; } }

    public Puzzle puzzle;
    public List<Skill> skills;

#region Game State Controll

    public void StartGame(StageData _stage)
    {
        stage = _stage;

        if(state == PlayState.Ready && isGameover)
        {
            state = PlayState.Play;
            isGameover = false;
            //factory = new TileFactory(tilePrefab, board);

            currentAttackCount = stage.AttackLimit;
            currentMonsterCount = stage.MonsterCount;
            monsters = stage.monsters.ToArray();
            //StartCoroutine(CreateBoard(stage.BoardSize));

            skills = Database.Instance.Skills;

            puzzle = new Puzzle(stage, board);
            StartCoroutine(puzzle.CreatePuzzle(tilePrefab));
        }
    }

    public void PauseGame()
    {
        if(state == PlayState.Pause)
        {
            return;
        }

        settingView.Play("Setting_Show");
        state = PlayState.Pause;
    }

    public void ResumeGame()
    {
        if (state != PlayState.Pause)
        {
            return;
        }

        settingView.Play("Setting_Close");
        state = PlayState.Play;
    }

    public void RestartGame()
    {
        FinishGame();
        StartGame(stage);
    }

    public void FinishGame()
    {
        settingView.Play("Setting_Idle");
        state = PlayState.Ready;
        isGameover = true;

        puzzle.Clean();

        //DeleteBoard();
    }
    #endregion

    #region Board Controll
    /*
    public IEnumerator CreateBoard(int _boardSize)
    {
        tiles = new List<Tile>();
        tileSize = boardWidth / _boardSize - spacing;
        startX = -(tileSize / 2f) * (_boardSize - 1);
        startY = -startX;

        var types = new List<TileData.TileType>();

        // 리스트 생성
        SetTypeList(types, _boardSize);

        // 섞기
        Shuffle(types, _boardSize);

        int monsterIndex = 0;

        // 배치
        for (int i = 0; i < types.Count; i++)
        {
            var coord = IndexToCoord(i);
            var pos = CoordToPosition(coord);

            float rate = 0f;

            if(types[i].Equals(TileData.TileType.Monster))
            {
                rate = monsters[monsterIndex++];
            }

            tiles.Add(factory.Create(types[i], rate, pos, tileSize, i));

            // 생성 딜레이
            yield return new WaitForSeconds(0.05f);
        }

        // 마지막 빈칸
        tiles.Add(null);

        // 빈타일 인덱스 설정
        blankIndex = tiles.Count - 1;

        // 게임시작
        StartCoroutine(Game());
    }

    private void SetTypeList(List<TileData.TileType> _list, int _boardSize)
    {
        int tileCount = (_boardSize * _boardSize) - 1;
        int monsterCount = currentMonsterCount;
        int swordCount = monsterCount * 2;

        // 타일 리스트 생성
        for (int i = 0; i < tileCount; i++)
        {
            if (i < monsterCount)
            {
                _list.Add(TileData.TileType.Monster);
            }
            else if (i - monsterCount < swordCount)
            {
                _list.Add(TileData.TileType.Attack);
            }
            else
            {
                _list.Add(TileData.TileType.Normal);
            }
        }
    }

    private void Shuffle(List<TileData.TileType> _list, int _size)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            int index1 = i;
            int index2 = Random.Range(0, _list.Count);
            var coord1 = IndexToCoord(index1);
            var coord2 = IndexToCoord(index2);

            bool isEdge1 =
                (coord1.x == 0
                || coord1.y == 0
                || coord1.x == _size - 1
                || coord1.y == _size - 1);

            bool isEdge2 =
                (coord2.x == 0
                || coord2.y == 0
                || coord2.x == _size - 1
                || coord2.y == _size - 1);

            if (   (_list[index1].Equals(TileData.TileType.Attack) && !isEdge2)
                || (_list[index2].Equals(TileData.TileType.Attack) && !isEdge1)
                || (_list[index1].Equals(TileData.TileType.Monster) && isEdge2)
                || (_list[index2].Equals(TileData.TileType.Monster) && isEdge1))
            {
                // 다시 선택
                i--;
                continue;
            }

            //Debug.Log(_list[index1] + "(" + index1 + ") -> " + _list[index2] + "(" + index2 + ")");

            TileData.TileType temp = _list[index1];
            _list[index1] = _list[index2];
            _list[index2] = temp;
        }
    }

    public void DeleteBoard()
    {
        int count = board.transform.childCount;

        for (int i = 0; i < count; i++)
        {
            Destroy(board.transform.GetChild(i).gameObject);
        }

        tiles.Clear();
    }
    */

    #endregion

    #region Tile Movement

    /*
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
    }

    private void MoveVertical(int _selectIndex, Vector2 _selectCoord, Vector2 _blankCoord)
    {
        isChanged = true;

        int count = (int)Mathf.Abs(_blankCoord.y - _selectCoord.y);

        for (int i = count - 1; i >= 0; i--)
        {
            int index = _selectIndex - stage.BoardSize * i;
            int target = _selectIndex - stage.BoardSize * (i + 1);

            if (_blankCoord.y > _selectCoord.y)
            {
                index = _selectIndex + stage.BoardSize * i;
                target = _selectIndex + stage.BoardSize * (i + 1);
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
        int x = _index == 0 ? 0 : _index % stage.BoardSize;
        int y = _index == 0 ? 0 : _index / stage.BoardSize;

        return new Vector2(x, y);
    }

    public int CoordToIndex(Vector2 _coord)
    {
        return (int)(_coord.y * stage.BoardSize + _coord.x);
    }

    public Vector2 CoordToPosition(Vector2 _coord)
    {
        return new Vector2(startX + tileSize * _coord.x, startY - tileSize * _coord.y);
    }
    */

    #endregion

    #region Tile Attack
    /*
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
        DeleteBoard();
        StartCoroutine(CreateBoard(stage.BoardSize));
    }
    */

    public int reinforceScope;

    public void Attack()
    {
        puzzle.Attack();
    }

    public void UseSkill(int _index)
    {
        skills[_index].Activate();
    }

#endregion

    private IEnumerator Game()
    {
        while (!isGameover)
        {
            // 스테이지 조건이 있으면
            if(stage.isAchieve)
            {
                // 현재 횟수제한이 0이면 조건 종료
                if (stage.AttackLimit <= 0)
                {
                    state = PlayState.Failed;
                    DialogManager.Instance.ShowDialog("Failed");
                }
            }

            // 몬스터 수가 0이면 성공
            if (currentMonsterCount == 0)
            {
                // 결과 화면 출력
                state = PlayState.Clear;
                DialogManager.Instance.ShowDialog("Clear");
            }

            yield return null;
        }
    }

    public void ChangeTile(System.Type _type)
    {
        puzzle.ChangeTile(_type);
    }

    /*
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
                    || coord.x >= stage.BoardSize
                    || coord.y < 0
                    || coord.y >= stage.BoardSize)
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
    */
}
