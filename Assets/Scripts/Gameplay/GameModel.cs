using System;
using System.Collections.Generic;
using Data.Catalog;
using Gameplay.Hero;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class GameModel : IGameModel, ITickable
    {
        public HeroModel HeroModel { get; }
        
        private readonly CatalogDataRepository _catalogDataRepository;
        
        private List<BaseAbilityModel> Abilities = new List<BaseAbilityModel>();
        private List<BaseAbilityModel> abilitiesToRemove = new List<BaseAbilityModel>();

        public GameModel(
            CatalogDataRepository catalogDataRepository, 
            ProjectConfig config)
        {
            _catalogDataRepository = catalogDataRepository;

            HeroModel = new HeroModel(config);
        }

        public void ApplyBonus(BonusData data)
        {
            foreach (var abilityId in data.abilitiesIds)
            {
                var abilityData = _catalogDataRepository.Abilities.Get(abilityId);
                ApplyAbility(abilityData);
            }
        }

        private void ApplyAbility(AbilityData abilityData)
        {
           // if (Enum.TryParse( abilityData.abilityType, out AbilityType abilityType) == false);
           // {
               // Debug.LogError($"fail to parse ability type : {abilityData.abilityType}");
           // }
                
           var abilityModel = CreateAbility(abilityData.abilityType, abilityData);
           if (abilityModel == default)
               return;
           
           //TODO make SimpleParamValueAbility -> drop active abilities with same param name 
           foreach (var ability in Abilities)
           {
               ability.Finish();
           }
           
           Abilities.Add(abilityModel);
           abilityModel.Start();
           
           // TODO Fire signal to show UI with ability title/description
           
        }

        private BaseAbilityModel CreateAbility(AbilityType abilityType, AbilityData abilityData)
        {
            //TODO make SimpleParamValueAbility -> change value by param name
            switch (abilityType)
            { 
                case AbilityType.Fly: return new FlyAbilityModel(HeroModel, abilityData);
                case AbilityType.SpeedUp: return new SpeedUpAbilityModel(HeroModel, abilityData);
                case AbilityType.SlowDown: return new SlowDownAbilityModel(HeroModel, abilityData);
                default: return default;
            }
        }

        void ITickable.Tick()
        {
            if (abilitiesToRemove.Count > 0)
            {
                foreach (var abilityModel in abilitiesToRemove)
                {
                    Abilities.Remove(abilityModel);
                }
                abilitiesToRemove.Clear();
            }
            
            foreach (var ability in Abilities)
            {
                ability.Update();
                if (ability.IsFinished)
                {
                    abilitiesToRemove.Add(ability);
                }
            }
        }
    }
}