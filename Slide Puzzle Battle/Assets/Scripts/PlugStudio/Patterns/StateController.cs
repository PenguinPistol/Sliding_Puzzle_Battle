using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace com.PlugStudio.Patterns
{
    public class StateController : Singleton<StateController>
    {
        private const string STATE_CHANGE_ANIMATION = "StateChange";
        private const string STATE_CLOSE_ANIMATION = "StateClose";

        [SerializeField]
        private List<State> stateList;
        private Dictionary<string, State> stateDictionary;

        [SerializeField]
        private string currentStateName;
        [SerializeField]
        private string beforeStateName;
        private State currentState;

        public Animator changeAnimation;

        public string CurrentStateName { get { return currentStateName; } }
        public State CurrentState { get { return currentState; } }
    
        private void Start()
        {
            stateDictionary = new Dictionary<string, State>();

            for (int i = 0; i < stateList.Count; i++)
            {
                stateList[i].Controller = this;
                stateDictionary.Add(stateList[i].GetType().Name, stateList[i]);
            }

            if(stateList.Count > 0)
            {
                currentState = stateList[0];

                currentStateName = currentState.GetType().Name;
                currentState.Init();
            }
        }
    
        void Update()
        {
            //Debug.Log(" : " + changeAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime);
            if (currentState != null)
            {
                currentState.Execute();
            }
        }

        public void ChangeState(string _name, params object[] _datas)
        {
            StartCoroutine(Change(_name, _datas));
        }

        public void ChangeBeforeState(params object[] _datas)
        {
            StartCoroutine(ChangeBefore(_datas));
        }

        public void ReloadState(params object[] _datas)
        {
            currentState.Init(_datas);
        }

        private IEnumerator Change(string _name, params object[] _datas)
        {
            float time = 0f;

            changeAnimation.Play(STATE_CLOSE_ANIMATION);

            while (time < changeAnimation.GetCurrentAnimatorStateInfo(0).length)
            {
                time += Time.deltaTime;
                yield return null;
            }

            beforeStateName = currentStateName;

            if (currentState != null)
            {
                currentState.canvas.SetActive(false);
                currentState.Exit();
            }

            currentState = stateDictionary[_name];

            if (currentState == null)
            {
                Debug.Log(_name + " State is Not Found");
                currentState = stateDictionary[beforeStateName];
                beforeStateName = "";
            }

            currentState.canvas.SetActive(true);

            time = 0f;
            changeAnimation.Play(STATE_CHANGE_ANIMATION);
            currentStateName = currentState.GetType().Name;

            while (time < changeAnimation.GetCurrentAnimatorStateInfo(0).length)
            {
                time += Time.deltaTime;
                yield return null;
            }

            currentState.Init(_datas);
        }

        private IEnumerator ChangeBefore(params object[] datas)
        {
            float time = 0f;

            changeAnimation.Play(STATE_CLOSE_ANIMATION);

            while (time < changeAnimation.GetCurrentAnimatorStateInfo(0).length)
            {
                time += Time.deltaTime;
                yield return null;
            }

            if (currentState != null)
            {
                currentState.canvas.SetActive(false);
                currentState.Exit();
            }

            currentState = stateDictionary[beforeStateName];

            if (currentState == null)
            {
                Debug.Log("Before State is Null");
                currentState = stateDictionary[currentStateName];
                beforeStateName = "";
            }

            currentState.canvas.SetActive(true);
            currentStateName = currentState.GetType().Name;

            time = 0f;
            changeAnimation.Play(STATE_CHANGE_ANIMATION);

            while (time < changeAnimation.GetCurrentAnimatorStateInfo(0).length)
            {
                time += Time.deltaTime;
                yield return null;
            }
            currentState.Init(datas);
        }
    }
}
