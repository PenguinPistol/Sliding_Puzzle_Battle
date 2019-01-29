using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.PlugStudio.Patterns;

public class Database : Singleton<Database>
{
    private List<StageData> stageData;

    public List<StageData> StageData { get { return stageData; } }

    public void ReadStageData()
    {
        stageData = new List<StageData>();

        // 실제 데이터 읽기

        for (int i = 0; i < 10; i++)
        {
            int size = Random.Range(0, 5) + 4;

            var data = new StageData(size, 0, 0, 2);
            stageData.Add(data);
        }
    }
}
