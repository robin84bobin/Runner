using System;
using UnityEngine;


    public class Timer
    {
        private readonly float _delay;
        private readonly float _period;
        private readonly float _totalTime;
        public float Delay { get { return _delay; } }

        private float _startTime;
        private float _periodTime;
        private float _finishTime;

        private Action _periodCallback;
        private Action _startCallback;
        private Action _finishCallback;


        public Timer(float totalTime, float period = 0f, float delay = 0f)
        {
            _delay = delay;
            _period = period;
            _totalTime = totalTime;
        }

        public Timer OnStart(Action startCallback)
        {
            _startCallback = startCallback;
            return this;
        }

        public Timer OnPeriod(Action periodCallback)
        {
            _periodCallback = periodCallback;
            return this;
        }

        public Timer OnComplete(Action finishCallback)
        {
            _finishCallback = finishCallback;
            return this;
        }

        public void Start()
        {
            _startTime = Time.time + _delay;
            _finishTime = _totalTime + Time.time;
            if (_period > 0) {
                _periodTime = _period + _startTime;
            }

        }


        public void Update()
        {
            if (_startTime > 0 && Time.time >= _startTime) {
                OnStart();
            }

            if (_periodTime > 0 && Time.time >= _periodTime) {
                OnPeriod();
            }
            
            if (_finishTime > 0 && Time.time >= _finishTime) {
                Complete();
            }
        }

        private void OnStart()
        {
            _startCallback?.Invoke();
            _startTime = 0f;
        }

        private void OnPeriod()
        {
            _periodCallback?.Invoke();

            if (_period > 0) {
                _periodTime = _period + Time.time;
            }
        }

        private void Complete()
        {
            _periodTime = 0f;
            _finishTime = 0f;

            _finishCallback?.Invoke();

        }


        public void Release ()
        {
            _periodCallback = null;
            _startCallback = null;
            _finishCallback = null;
        }
}
