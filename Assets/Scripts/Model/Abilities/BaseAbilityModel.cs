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
        
        protected readonly AbilitiesModel AbilitiesModel;
        private float _startTime;
        private float _finishTime;


        protected BaseAbilityModel(AbilitiesModel abilitiesModel, AbilityData data)
        {
            AbilitiesModel = abilitiesModel;
            Data = data;
        }

        public virtual void Start()
        {
            _startTime = Time.time;
            _finishTime = _startTime + Data.duration;
            Debug.Log($"Ability {Data.title} Start!");
        }

        public void Update()
        {
            if (Time.time >= _finishTime)
            {
                Finish();
            }
            else
            {
                TotalSeconds = _finishTime - Time.time;
            }
        }

        public virtual void Finish()
        {
            IsFinished = true;
            Debug.Log($"Ability {Data.title} Finished!");
        }
    }
}