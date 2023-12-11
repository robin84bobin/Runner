using System.Collections.Generic;
using Data.Catalog;
using Gameplay.Hero;
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
            GameplayConfig config)
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
           var newAbility = CreateAbility(abilityData.abilityType, abilityData);
           if (newAbility == default)
               return;
           
           foreach (var ability in Abilities)
           {
               if (ability.ParamName == newAbility.ParamName)
               {
                   ability.Finish();
               }
           }
           
           Abilities.Add(newAbility);
           newAbility.Start();
        }

        private BaseAbilityModel CreateAbility(AbilityType abilityType, AbilityData abilityData)
        {
            switch (abilityType)
            { 
                case AbilityType.Fly: 
                case AbilityType.SpeedUp: 
                case AbilityType.SlowDown: 
                    return new PlayerParamSimpleAbilityModel(HeroModel, abilityData);
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