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

    private bool isPlaying;
    private PlayState state;

    // 남은 제한시간
    private float currentTimeout;
    // 남은 공격횟수
    private int currentAttackCount;
    // 남아있는 몬스터 수
    private int monsterCount;

    // 제한시간 --> stage data
    private float originTimeout;
    // 공격횟수 --> stage data
    private int originAttackCount;
    // 달성조건 갯수 --> stage data
    private int achieveCount;
    // 보드 크기 --> stage data
    int boardSize = 5;

    // 현재 스테이지 정보
    private StageData stage;

    // 보드 
    public GridLayoutGroup board;
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

        if(state == PlayState.Ready && !isPlaying)
        {
            state = PlayState.Play;
            isPlaying = true;
            factory = new TileFactory(tilePrefab, board.transform);

            //boardSize = Database.Instance....
            // 1000 / boardSize - 20;

            CreateBoard(stage.boardSize);

            StartCoroutine(Game());
        }
    }

    public void CreateBoard(int _boardSize)
    {
        DeleteBoard();

        float cellSize = boardWidth / _boardSize - spacing;

        board.cellSize = new Vector2(cellSize, cellSize);
        board.constraintCount = _boardSize;
        board.spacing = new Vector2(spacing, spacing);

        int tileCount = (int)(Mathf.Pow(_boardSize, 2) - 1);

        for (int i = 0; i < tileCount; i++)
        {
            var types = System.Enum.GetValues(typeof(TileFactory.TileType));
            var type = (TileFactory.TileType)Random.Range(0, types.Length);

            factory.Create(type, 1);
        }
    }

    private void DeleteBoard()
    {
        int count = board.transform.childCount;
        
        for (int i = 0; i < count; i++)
        {
            Destroy(board.transform.GetChild(i).gameObject);
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
        isPlaying = false;

        DeleteBoard();
    }


    private IEnumerator Game()
    {
        while(isPlaying)
        {
            // 시간제한
            // 시간제한이 0이면 시간제한 없음
            if(originTimeout > 0)
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
            if(originAttackCount > 0)
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
