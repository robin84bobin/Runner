using System;
using Data.Catalog;
using Gameplay.Hero;
using UnityEngine;

namespace Gameplay
{
    public abstract class BaseAbilityModel
    {
        public bool IsFinished { get; private set; }
        protected readonly HeroModel _heroModel;
        protected readonly  AbilityData _data;

        protected float _startTime;
        protected float _finishTime;

        protected BaseAbilityModel(HeroModel heroModel, AbilityData data)
        {
            _heroModel = heroModel;
            _data = data;
        }

        public virtual void Start()
        {
            _startTime = Time.time;
            _finishTime = _startTime + _data.duration;
            Debug.Log($"Ability {_data.title} Start!");
        }

        public void Update()
        {
            if (Time.time >= _finishTime)
            {
                Finish();
            }
            else
            {
                double totalSeconds = _finishTime - Time.time;
                TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
                Debug.Log($"Ability {_data.title} time: {time.ToString("hh':'mm':'ss")}");
            }
        }

        public virtual void Finish()
        {
            IsFinished = true;
            Debug.Log($"Ability {_data.title} Finished!");
        }
    }
}