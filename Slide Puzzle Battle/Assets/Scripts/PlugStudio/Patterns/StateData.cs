using System.Collections.Generic;
using UnityEngine;
using System;
using com.PlugStudio.Patterns;

namespace com.PlugStudio.Patterns
{
    public class StateData
    {
        [SerializeField]
        private Dictionary<string, object> datas;

        [SerializeField]
        private State currentState;
        private State afterState;

        public State CurrentState { get { return currentState; } }
        public State AfterState { get { return afterState; } }

        public StateData(State _currentState, State _afterState)
        {
            datas = new Dictionary<string, object>();

            currentState = _currentState;
            afterState = _afterState;
        }

        public void PutData(string _name, object _data)
        {
            datas.Add(_name, _data);
        }

        public object GetData(string _name)
        {
            return datas[_name];
        }
    }

}