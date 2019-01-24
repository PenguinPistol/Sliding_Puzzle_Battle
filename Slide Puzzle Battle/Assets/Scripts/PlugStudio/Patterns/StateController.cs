using UnityEngine;
using System.Collections.Generic;

namespace com.PlugStudio.Patterns
{
    public class StateController : Singleton<StateController>
    {
        [SerializeField]
        private List<State> stateList;
        private Dictionary<string, State> stateDictionary;

        [SerializeField]
        private string currentStateName;
        [SerializeField]
        private string beforeStateName;

        public string CurrentStateName { get { return currentStateName; } }

        private State currentState;

        public State CurrentState
        {
            get { return currentState; }
        }
    
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
            if (currentState != null)
                currentState.Execute();
        }

        public void ChangeState(string _name)
        {
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
            currentState.Init();
            currentStateName = currentState.GetType().Name;
        }

        public void ChangeState(string _name, params object[] datas)
        {
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
            currentState.Init(datas);
            currentStateName = currentState.GetType().Name;
        }

        public void ChangeBeforeState(params object[] datas)
        {
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
            currentState.Init();
            currentStateName = currentState.GetType().Name;
        }
    }
}
