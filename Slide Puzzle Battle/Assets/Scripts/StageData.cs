using System;

public class StageData
{
    public enum StageState
    {
        Lock, Unlock, Clear
    }

    // 보드 크기
    public int boardSize;
    // 제한시간
    public float timeout;
    // 공격횟수
    public int attackCount;
    // 몬스터 수
    public int monsterCount;
    // 달성조건 개수
    public bool isAchieve;
    // 스테이지 상태
    public StageState state;

    public StageData(int _boardSize, float _timeout, int _attackCount, int _monsterCount)
    {
        boardSize = _boardSize;
        timeout = _timeout;
        attackCount = _attackCount;
        monsterCount = _monsterCount;

        if(attackCount != -1)
        {
            isAchieve = true;
        }

        state = StageState.Lock;
    }
}
