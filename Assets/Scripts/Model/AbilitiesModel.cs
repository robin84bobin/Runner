using System;
using System.Collections.Generic;
using Data.Catalog;
using Model.Abilities;
using Zenject;

namespace Model
{
    /// <summary>
    /// create,store and remove active abilities
    /// </summary>
    public class AbilitiesModel : ITickable
    {
        public event Action OnAbilitiesUpdate;
        public List<BaseAbilityModel> Abilities { get; } = new List<BaseAbilityModel>();
        
        private readonly CatalogDataRepository _catalogDataRepository;
        private readonly HeroModel _heroModel;
        private readonly List<BaseAbilityModel> _abilitiesToRemove = new List<BaseAbilityModel>();

        public AbilitiesModel(CatalogDataRepository catalogDataRepository, HeroModel heroModel)
        {
            _catalogDataRepository = catalogDataRepository;
            _heroModel = heroModel;
        }

        public void ApplyAbilities(string bonusId)
        {
            var bonusData = _catalogDataRepository.Bonuses.Get(bonusId);
            foreach (var abilityId in bonusData.abilitiesIds)
            {
                var abilityData = _catalogDataRepository.Abilities.Get(abilityId);
                ApplyAbility(abilityData);
            }
        }

        private void ApplyAbility(AbilityData abilityData)
        {
            var newAbility = CreateAbility(abilityData.abilityType, abilityData);
            if (newAbility == default)
                return;

            newAbility.Start();
            Abilities.Add(newAbility);
        }

        private BaseAbilityModel CreateAbility(AbilityType abilityType, AbilityData abilityData)
        {
            switch (abilityType)
            { 
                case AbilityType.Fly: 
                case AbilityType.SpeedUp: 
                case AbilityType.SlowDown: 
                    return new PlayerParamSimpleAbilityModel(_heroModel, this, abilityData);
                default: return default;
            }
        }

        void ITickable.Tick()
        {
            if (_abilitiesToRemove.Count > 0)
            {
                foreach (var abilityModel in _abilitiesToRemove)
                {
                    Abilities.Remove(abilityModel);
                }
                _abilitiesToRemove.Clear();
            }
            
            foreach (var ability in Abilities)
            {
                ability.Update();
                if (ability.IsFinished)
                {
                    _abilitiesToRemove.Add(ability);
                }
            }
            
            OnAbilitiesUpdate?.Invoke();
        }
    }
}