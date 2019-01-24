using UnityEngine;
using com.PlugStudio.Input;

namespace com.PlugStudio.Patterns
{
    public abstract class State : MonoBehaviour, ITouchObservable
    {
        //private StateController controller;
        //public StateController Controller { get { return controller; } set { controller = value; } }

        public GameObject stateUI;

        public abstract void Init(params object[] datas);
        public abstract void Execute();
        public abstract void Exit();

        // ----------- InputObservable Interface ---------------
        public virtual void TouchBegan(Vector3 _touchPosition, int _index)    {}
        public virtual void TouchMoved(Vector3 _touchPosition, int _index)    {}
        public virtual void TouchCancel(Vector3 _touchPosition, int _index)   {}
        public virtual void TouchEnded(Vector3 _touchPosition, int _index)    {}
    }
}
