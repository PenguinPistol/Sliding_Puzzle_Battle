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
    public float TimeLimit;
    // 공격횟수
    public int AttackLimit;
    // 몬스터 수
    public int MonsterCount;
    // 달성조건 개수
    public bool[] isAchieve;
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

    public StageData(int _boardSize, float _timeLimit, int _attackCount, int _monsterCount)
    {
        BoardSize = _boardSize;
        TimeLimit = _timeLimit;
        AttackLimit = _attackCount;
        MonsterCount = _monsterCount;

        isAchieve = new bool[] { false, false };

        if (AttackLimit != 0)
        {
            isAchieve[0] = true;
        }

        if(TimeLimit != 0)
        {
            isAchieve[1] = true;
        }

        state = StageState.Lock;
        monsters = new List<int>();
    }
}
