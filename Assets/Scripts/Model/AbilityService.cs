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
    public class AbilityService : ITickable
    {
        public event Action OnAbilitiesUpdate;
        public List<BaseAbilityModel> Abilities { get; } = new List<BaseAbilityModel>();
        
        private readonly CatalogDataRepository _catalogDataRepository;
        private readonly List<BaseAbilityModel> _abilitiesToRemove = new List<BaseAbilityModel>();

        public AbilityService(CatalogDataRepository catalogDataRepository)
        {
            _catalogDataRepository = catalogDataRepository;
        }

        public void ApplyAbilities(string bonusId, ActorModel actorModel)
        {
            var bonusData = _catalogDataRepository.Bonuses.Get(bonusId);
            foreach (var abilityId in bonusData.abilitiesIds)
            {
                var abilityData = _catalogDataRepository.Abilities.Get(abilityId);
                ApplyAbility(abilityData, actorModel);
            }
        }

        private void ApplyAbility(AbilityData abilityData, IActorParamContainer actor)
        {
            var newAbility = CreateAbility(abilityData.abilityType, abilityData, actor);
            if (newAbility == default)
                return;

            OnPrepareAbilityStart(abilityData);
            
            newAbility.StartAbility();
            Abilities.Add(newAbility);
        }

        private void OnPrepareAbilityStart(AbilityData newAbilityData)
        {
            switch (newAbilityData.abilityType)
            {
                case AbilityType.ActorParamUniqueAbility:
                    TryDropSameParameterAbilities(newAbilityData);
                    break;
            }
        }
        
        private void TryDropSameParameterAbilities(AbilityData newAbilityData)
        {
            foreach (var ability in Abilities)
            {
                if (ability.Data.paramType == newAbilityData.paramType)
                {
                    ability.FinishAbility();
                }
            }
        }
        

        private BaseAbilityModel CreateAbility(AbilityType abilityType, AbilityData abilityData, IActorParamContainer actor)
        {
            switch (abilityType)
            { 
                case AbilityType.ActorParamUniqueAbility: 
                    return new ActorParameterUniqueAbility(actor, abilityData);
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
                ability.UpdateTime();
                if (ability.IsFinished)
                {
                    _abilitiesToRemove.Add(ability);
                }
            }
            
            OnAbilitiesUpdate?.Invoke();
        }
    }
}