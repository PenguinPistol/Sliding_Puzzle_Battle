using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.PlugStudio.Patterns;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum PlayState
    {
        Ready, Play, Pause, Attack, Finish
    }

    public static int Reinforce = 1;
    public static bool TimeStop = false;

    private Database db;
    private PlayState state;
    private int currentLevel;
    private PlayState beforeState;
    private bool isContinued;

    public GameObject tutorial;

    public Database.SavedGameData GameData    { get { return db.GameData; } }
    public List<StageData>        Stages      { get { return db.Stages; } }
    public List<Skill>            Skills      { get { return db.Skills; } }
    public PlayState              State       { get { return state; } }
    public bool                   IsPlaying   { get { return state == PlayState.Play; } }
    public bool                   IsPause     { get { return state == PlayState.Pause; } }
    public int                    BoardSize   { get { return Stages[currentLevel].BoardSize; } }
    public int                    CompleteLevel { get { return db.GameData.completeLevel; } }
    public int                    LastPlayedLevel { get { return currentLevel; } }
    public bool                   IsContinued { get { return isContinued; } }

    public bool IsViewTutorial {
        get { return db.GameData.viewTutorial; }
        set { db.GameData.viewTutorial = value; }
    }

    [HideInInspector]
    public Puzzle currentPuzzle;

    private IEnumerator Start()
    {
        DontDestroyOnLoad(gameObject);

        var overlaps = FindObjectsOfType<GameManager>();

        for (int i = 1; i < overlaps.Length; i++)
        {
            Destroy(overlaps[i].gameObject);
        }

        db = new Database();

        StateController.Instance.Init();
        StateController.Instance.ChangeState("Intro", false);

        float time = Time.time;

        yield return StartCoroutine(db.ReadGameConst());
        db.LoadGameData();

        yield return StartCoroutine(db.ReadStageData());
        yield return StartCoroutine(db.ReadLevelMonsterInfo());

        SkillManager.Instance.Init();

        Debug.Log("elapse time : " + (Time.time - time));

        StateController.Instance.ChangeState("StageSelect", false);
        currentLevel = GameData.completeLevel;
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            db.SaveGameData();

            if (isDelete)
            {
                PlayerPrefs.DeleteAll();
            }
        }
    }

    private void OnApplicationQuit()
    {
        if(db != null)
        {
            db.SaveGameData();
        }

        if(isDelete)
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void LoadLevel(int _level)
    {
        if(state == PlayState.Ready)
        {
            isContinued = false;
            currentLevel = _level;
            StateController.Instance.ChangeState("Game", true, _level);
        }
    }

    public void StartGame()
    {
        if(state == PlayState.Ready)
        {
            state = PlayState.Play;
        }
    }

    public void PauseGame(bool _isPause)
    {

        if (_isPause)
        {
            // pause
            beforeState = state;
            ChangeState(PlayState.Pause);
        }
        else
        {
            // resume
            ChangeState(beforeState);
        }
    }

    public void ContinueGame()
    {
        if(isContinued)
        {
            return;
        }

        isContinued = true;
        ((Game)StateController.Instance.CurrentState).ContinueGame();
    }

    public void FinishGame(bool _isClear)
    {
        ChangeState(PlayState.Finish);

        if (_isClear)
        {
            if(currentLevel >= CompleteLevel)
            {
                if(currentLevel < Stages.Count - 1)
                {
                    db.GameData.completeLevel = currentLevel + 1;
                }
            }

            DialogManager.Instance.ShowDialog("Clear");
        }
        else
        {
            DialogManager.Instance.ShowDialog("Failed");
        }

        AdsManager.Instance.ShowInterstitial();
    }

    public void RestartLevel()
    {
        ChangeState(PlayState.Ready);

        LoadLevel(currentLevel);
    }


    public void PlayNextLevel()
    {
        ChangeState(PlayState.Ready);

        LoadLevel(currentLevel + 1);
    }

    public void LeaveLevel()
    {
        ChangeState(PlayState.Ready);

        StateController.Instance.ChangeState("StageSelect", true);
    }

    public void ChangeState(PlayState _state)
    {
        state = _state;
    }

    public void ShowDialog(string _name)
    {
        DialogManager.Instance.ShowDialog(_name);
    }

    public void ShowTutorial()
    {
        tutorial.SetActive(true);
    }

    private bool isDelete = false;

    public void DeleteData()
    {
        isDelete = true;

        Application.Quit();
    }

    public void ShowInterstial()
    {
        AdsManager.Instance.ShowInterstitial();
    }

    public void ShowEnergyRewardAd()
    {
        if(SkillManager.Instance.currentEnergy == GameConst.MaxEnergy)
        {
            Debug.Log("full charge energy");
            return;
        }

        AdsManager.Instance.ShowRewardEnergy();
    }
}
