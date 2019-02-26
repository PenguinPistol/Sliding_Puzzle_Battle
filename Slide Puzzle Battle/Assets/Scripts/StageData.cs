using System.Collections.Generic;

public class StageData
{
    public enum StageState
    {
        Lock, Unlock, Clear
    }

    // 보드 크기
    public int BoardSize;
    // 제한시간
    public float TimeLimitSec;
    // 공격횟수
    public int AttackLimit;
    // 몬스터 수
    public int MonsterCount;
    // 달성조건 개수
    public bool isAchieve;
    // 스테이지 상태
    public StageState state;
    // 몬스터체력
    public List<int> monsters;
    // 레벨 인덱스
    public int level;

    public StageData()
    {
        state = StageState.Lock;
        monsters = new List<int>();
    }

    public StageData(int _boardSize, float _timeLimitSec, int _attackCount, int _monsterCount)
    {
        BoardSize = _boardSize;
        TimeLimitSec = _timeLimitSec;
        AttackLimit = _attackCount;
        MonsterCount = _monsterCount;

        if(AttackLimit != -1)
        {
            isAchieve = true;
        }

        state = StageState.Lock;
        monsters = new List<int>();
    }
}
