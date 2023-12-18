using Data.Catalog;
using UnityEngine;

namespace Model.Abilities
{
    /// <summary>
    /// base class for ability functionality
    /// controls life time of ability
    /// </summary>
    public abstract class BaseAbilityModel
    {
        public bool IsFinished { get; private set; }
        public AbilityData Data { get; }
        public float TotalSeconds { get; private set; }
        
        private float _startTime;
        private float _finishTime;


        protected BaseAbilityModel(AbilityData data)
        {
            Data = data;
        }

        public virtual void StartAbility()
        {
            _startTime = Time.time;
            _finishTime = _startTime + Data.duration;
            Debug.Log($"Ability {Data.title} StartAbility!");
        }

        public void OnTick()
        {
            if (Time.time >= _finishTime)
            {
                FinishAbility();
            }
            else
            {
                TotalSeconds = _finishTime - Time.time;
            }
        }

        public virtual void FinishAbility()
        {
            IsFinished = true;
            Debug.Log($"Ability {Data.title} Finished!");
        }
    }
}