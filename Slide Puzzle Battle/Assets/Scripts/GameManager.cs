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
    private float currentTimeLimit;

    // 현재 스테이지 정보
    private StageData stage;

    // 보드 
    public Transform board;
    // 타일 프리팹
    public TileController tilePrefab;
    // 설정 화면 애니메이터
    public Animator settingView;

    public int AttackLimit { get { return currentAttackCount; } }
    public float TimeLimit { get { return currentTimeLimit; } }
    public PlayState State { get { return state; } }

    public Puzzle puzzle;
    public List<Skill> skills;
    public int completeLevel;

    private void Start()
    {
        skills = Database.Instance.Skills;
        completeLevel = Database.Instance.CompleteLastLevel;
    }

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
            currentTimeLimit = stage.TimeLimit;
            currentMonsterCount = stage.MonsterCount;


            puzzle = new Puzzle(stage, board);
            StartCoroutine(puzzle.CreatePuzzle(tilePrefab));
        }
    }

    public void PauseGame()
    {
        if(state != PlayState.Play)
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

    public int reinforceScope;

    public void Attack()
    {
        if(state != PlayState.Play || puzzle.isAttacked)
        {
            return;
        }

        StartCoroutine(puzzle.Attack());
    }

    public void UseSkill(int _index)
    {
        if (state != PlayState.Play)
        {
            return;
        }

        skills[_index].Activate();
    }

    public IEnumerator Game()
    {
        while (!isGameover)
        {
            if(state == PlayState.Pause)
            {
                yield return null;
                continue;
            }

            // 스테이지 조건이 있으면
            if(stage.isAchieve[0])
            {
                // 현재 횟수제한이 0이면 조건 종료
                if (stage.AttackLimit <= 0)
                {
                    state = PlayState.Failed;
                    DialogManager.Instance.ShowDialog("Failed");
                }
            }

            // 시간제한
            if (stage.isAchieve[1])
            {
                // 현재 횟수제한이 0이면 조건 종료
                if (currentTimeLimit <= 0)
                {
                    state = PlayState.Failed;
                    DialogManager.Instance.ShowDialog("Failed");
                    isGameover = true;
                }
                else
                {
                    currentTimeLimit -= Time.deltaTime;
                }
            }

            // 몬스터 수가 0이면 성공
            if (currentMonsterCount == 0)
            {
                // 결과 화면 출력
                isGameover = true;
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
}
