using System.Collections.Generic;

namespace Common.StateSwitcher
{
    public class BaseStateMachine <TKey,TState> where TState:IBaseState
    {
        private Dictionary<TKey,TState> _states;

        private TState _currentState;
        public TState CurrentState {
            get { return _currentState;}
        }

        public BaseStateMachine()
        {
            _states = new Dictionary<TKey, TState> ();
        }

        public void Add(TKey key,TState state)
        {
            if (!_states.ContainsKey (key)) {
                _states.Add (key, state);
            }
        }

        public void Remove(TKey key)
        {
            _states.Remove (key);
        }

        public TState SetState(TKey key)
        {
            if (_currentState != null)
            {
                if (_currentState.Equals(_states[key]))
                {
                    return _currentState;
                }
                _currentState.OnExitState();
            }

            _currentState = _states [key];
            _currentState.OnEnterState ();
            return _currentState;
        }
    }
}



