using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class Database
{
    private const string PATH_GAME_CONST            = "Data/GameConst";
    private const string PATH_LEVEL_DATA            = "Data/Info_Level";
    private const string PATH_LEVEL_MONSTER         = "Data/Info_Monster";
    private const string PATH_SKILL_INFO            = "Data/Info_Skill";

    private const string KEY_OPTION_BGM_MUTE        = "BGM_MUTE";
    private const string KEY_OPTION_SE_MUTE         = "SE_MUTE";
    private const string KEY_COMPLETE_LAST_LEVEL    = "COMPLETE_LAST_LEVEL";
    private const string KEY_REMAINING_ENERGY       = "REMAINING_ENERGY";
    private const string KEY_QUIT_TIME              = "QUIT_TIME";
    private const string KEY_VIEW_TUTORIAL          = "VIEW_TUTORIAL";

    private const string DATETIME_FORMAT            = "yyyyMMddHHmmss";

    private List<StageData> stageData;
    private List<Skill> skillList;
    private SavedGameData gameData;

    public List<StageData> Stages { get { return stageData; } }
    public List<Skill> Skills { get { return skillList; } }
    public SavedGameData GameData { get { return gameData; } }


    public class SavedGameData
    {
        public bool muteBGM;
        public bool muteSE;
        public long elapseTime;
        public int completeLevel;
        public int remainingEnergy;
        public bool viewTutorial;

        public SavedGameData(bool _muteBGM, bool _muteSE, long _elapseTime, int _completeLevel, int _remainingEnergy, bool _viewTutorial)
        {
            muteBGM = _muteBGM;
            muteSE = _muteSE;
            elapseTime = _elapseTime;
            completeLevel = _completeLevel;
            remainingEnergy = _remainingEnergy;
            viewTutorial = _viewTutorial;
        }
    }

    public void SaveGameData()
    {
        // 저장
        PlayerPrefs.SetString(KEY_OPTION_BGM_MUTE, gameData.muteBGM.ToString());
        PlayerPrefs.SetString(KEY_OPTION_SE_MUTE, gameData.muteSE.ToString());
        PlayerPrefs.SetInt(KEY_COMPLETE_LAST_LEVEL, gameData.completeLevel);
        PlayerPrefs.SetInt(KEY_REMAINING_ENERGY, SkillManager.Instance.currentEnergy);
        PlayerPrefs.SetString(KEY_QUIT_TIME, DateTime.Now.ToString(DATETIME_FORMAT));
        PlayerPrefs.SetString(KEY_VIEW_TUTORIAL, gameData.viewTutorial.ToString());

        PlayerPrefs.Save();
    }

    public void LoadGameData()
    {
        string currentTime = DateTime.Now.ToString(DATETIME_FORMAT);
        string quitTime = PlayerPrefs.GetString(KEY_QUIT_TIME, currentTime);

        string strMuteBGM = PlayerPrefs.GetString(KEY_OPTION_BGM_MUTE, bool.FalseString);
        string strMuteSE = PlayerPrefs.GetString(KEY_OPTION_SE_MUTE, bool.FalseString);
        string strViewTutorial = PlayerPrefs.GetString(KEY_VIEW_TUTORIAL, bool.FalseString);

        bool muteBGM = bool.Parse(strMuteBGM);
        bool muteSE = bool.Parse(strMuteSE);
        long elapseTime = long.Parse(currentTime) - long.Parse(quitTime);
        int completeLevel = PlayerPrefs.GetInt(KEY_COMPLETE_LAST_LEVEL, 0);
        int remainingEnergy = PlayerPrefs.GetInt(KEY_REMAINING_ENERGY, 5);
        bool viewTutorial = bool.Parse(strViewTutorial);

        gameData = new SavedGameData(muteBGM, muteSE, elapseTime, completeLevel, remainingEnergy, viewTutorial);

        int recoveryEnergy = (int)(elapseTime / GameConst.Cooldown_EnergyRecovery);
        float elapseRecoveryTime = elapseTime % GameConst.Cooldown_EnergyRecovery;

        remainingEnergy += recoveryEnergy;

        if (remainingEnergy >= GameConst.MaxEnergy)
        {
            remainingEnergy = GameConst.MaxEnergy;
            SkillManager.Instance.elapseRecoveryTime = 0f;
        }
        else
        {
            SkillManager.Instance.elapseRecoveryTime = elapseRecoveryTime;
        }

        SkillManager.Instance.currentEnergy = remainingEnergy;
    }
    
    // 레벨데이터 불러오기
    public IEnumerator ReadStageData()
    {
        var readData = CSVReader.ReadData(PATH_LEVEL_DATA);
        var types = readData["type"];
        var headers = readData["header"];
        int dataCount = readData[headers[0] + "_data"].Length;

        stageData = new List<StageData>();

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

            if(i == gameData.completeLevel)
            {
                data.state = StageData.StageState.Unlock;
            }
            else if(i < gameData.completeLevel)
            {
                data.state = StageData.StageState.Clear;
                data.isClear = true;
            }

            data.isAchieve = new bool[] {
                (data.AttackLimit != 0)
                , (data.TimeLimit != 0) };
            data.title = string.Format("LEVEL {0}", data.level);

            stageData.Add(data);

            yield return null;
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

            stageData[level-1].monsters.Add(hp);

            yield return null;
        }
    }

    public IEnumerator ReadGameConst()
    {
        var readData = CSVReader.ReadConst(PATH_GAME_CONST);

        GameConst gameConst = new GameConst();

        foreach (var data in readData)
        {
            string type = data["타입"];

            if(type.Equals("float"))
            {
                SetFieldData<GameConst, float>(gameConst, data["이름"], data["값"]);
            }
            else if (type.Equals("int"))
            {
                SetFieldData<GameConst, int>(gameConst, data["이름"], data["값"]);
            }
            else if (type.Equals("string"))
            {
                SetFieldData<GameConst, string>(gameConst, data["이름"], data["값"]);
            }

            yield return null;
        }
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
