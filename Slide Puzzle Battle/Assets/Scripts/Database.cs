using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.PlugStudio.Patterns;
using System;
using System.Reflection;

public class Database : Singleton<Database>
{
    private const string PATH_LEVEL_DATA = "Data/Info_Level";
    private const string PATH_LEVEL_MONSTER = "Data/Info_Monster";
    private const string PATH_SKILL_INFO = "Data/Info_Skill";

    private const string KEY_OPTION_BGM_MUTE = "BGM_MUTE";
    private const string KEY_OPTION_SE_MUTE = "SE_MUTE";
    private const string KEY_COMPLETE_LAST_LEVEL = "COMPLETE_LAST_LEVEL";
    private const string KEY_REMAINING_POWER = "REMAINING_POWER";
    private const string KEY_QUIT_TIME = "QUIT_TIME";

    private const string DATETIME_FORMAT = "yyyyMMddHHmmss";

    private List<StageData> stages;
    private int completeLastLevel = 1;
    private bool isLoadStage = false;
    private List<MonsterInfo> monsterInfo;
    private List<Skill> skills;

    public List<StageData> Stages { get { return stages; } }
    public List<Skill> Skills { get { return skills; } }
    public int CompleteLastLevel { get { return completeLastLevel; } }

    public bool StageLoaded { get { return isLoadStage; } }

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Save()
    {
        // 저장
        PlayerPrefs.SetInt(KEY_COMPLETE_LAST_LEVEL, completeLastLevel);
        PlayerPrefs.SetString(KEY_QUIT_TIME, DateTime.Now.ToString(DATETIME_FORMAT));
        PlayerPrefs.Save();
    }

    public IEnumerator Load()
    {
        // 시간 차이 계산하기
        string quit = PlayerPrefs.GetString(KEY_QUIT_TIME, "0");
        string now = DateTime.Now.ToString(DATETIME_FORMAT);
        long elapseTime = long.Parse(now) - long.Parse(quit);

        Debug.Log("Elapse Time : " + elapseTime);

        if (elapseTime >= 1000)
        {
            // 에너지 하나 충전
            // elapseTime / 1000 -> 충전량
        }

        completeLastLevel = PlayerPrefs.GetInt(KEY_COMPLETE_LAST_LEVEL, 1);

        yield return StartCoroutine(ReadStages());
        yield return StartCoroutine(ReadLevelMonsterInfo());
        yield return StartCoroutine(ReadSkillInfo());
    }

    // 레벨데이터 불러오기
    public IEnumerator ReadStages()
    {
        if (!isLoadStage)
        {
            float time = Time.time;

            var readData = CSVReader.ReadData(PATH_LEVEL_DATA);

            var types = readData["type"];
            var headers = readData["header"];

            int dataCount = readData[headers[0] + "_data"].Length;

            stages = new List<StageData>();

            for (int i = 0; i < dataCount; i++)
            {
                var data = new StageData
                {
                    level = i
                };

                for (int j = 0; j < headers.Length; j++)
                {
                    string value = readData[headers[j] + "_data"][i];
                    value.Trim();

                    if (value.Length <= 0)
                        continue;

                    if (types[j].Equals("float"))
                    {
                        SetFieldData<StageData, float>(data, headers[j].Trim(), value);
                    }
                    else if (types[j].Equals("int"))
                    {
                        SetFieldData<StageData, int>(data, headers[j].Trim(), value);
                    }
                    else if (types[j].Equals("string"))
                    {
                        SetFieldData<StageData, string>(data, headers[j].Trim(), value);
                    }
                }

                data.isAchieve = new bool[] { false, false };

                if (data.AttackLimit != 0)
                {
                    data.isAchieve[0] = true;
                }
                if (data.TimeLimit != 0)
                {
                    data.isAchieve[1] = true;
                }

                if (i < completeLastLevel)
                {
                    if (i == completeLastLevel - 1)
                    {
                        data.state = StageData.StageState.Unlock;
                    }
                    else
                    {
                        data.state = StageData.StageState.Clear;
                    }
                }

                stages.Add(data);

                yield return null;
            }

            Debug.Log("load time : " + (Time.time - time));
        }
    }

    public IEnumerator ReadLevelMonsterInfo()
    {
        var readData = CSVReader.ReadData(PATH_LEVEL_MONSTER);

        var types = readData["type"];
        var headers = readData["header"];

        int dataCount = readData[headers[0] + "_data"].Length;

        for (int i = 0; i < dataCount; i++)
        {
            int level = int.Parse(readData[headers[0] + "_data"][i]);
            int hp = int.Parse(readData[headers[1] + "_data"][i]);

            stages[level-1].monsters.Add(hp);

            yield return null;
        }

        isLoadStage = true;
        Debug.Log("Finished Read MonsterInfo");
    }

    public IEnumerator ReadSkillInfo()
    {
        var readData = CSVReader.ReadData(PATH_SKILL_INFO);

        var types = readData["type"];
        var headers = readData["header"];

        int dataCount = readData[headers[0] + "_data"].Length;

        skills = new List<Skill>();

        for (int i = 0; i < dataCount; i++)
        {
            var data = new Skill();

            for (int j = 0; j < headers.Length; j++)
            {
                string value = readData[headers[j] + "_data"][i];

                if (types[j].Equals("float"))
                {
                    SetFieldData<Skill, float>(data, headers[j].Trim(), value);
                }
                else if (types[j].Equals("int"))
                {
                    SetFieldData<Skill, int>(data, headers[j].Trim(), value);
                }
                else if (types[j].Equals("string"))
                {
                    SetFieldData<Skill, string>(data, headers[j].Trim(), value);
                }
            }

            skills.Add(data);

            yield return null;
        }

        Debug.Log("Finished Read Skill Info");
    }

    private void SetFieldData<T1, T2>(T1 _object, string _name, object _value)
    {
        //SetFieldData<MusicBeatData, float>(data, headers[i], value);
        Type type = typeof(T1);

        // 인스턴스 멤버 포함, 정적멤버 포함, 퍼블릭 포함, 논 퍼블릭 포함, 대소문자 구분안함
        var info = type.GetField(_name
            , BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);

        if(info == null)
        {
            return;
        }

        T2 value = (T2)Convert.ChangeType(_value, typeof(T2));

        info.SetValue(_object, value);
    }
}
