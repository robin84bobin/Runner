using Data.Catalog;
using Parameters;
using UnityEngine;

namespace Model.Abilities
{
    /// <summary>
    /// Ability that changes concrete hero parameter to new value
    /// and reset this ReactiveParameter to default value on finish.
    /// On start - drops existed abilities for same ReactiveParameter   
    /// </summary>
    public class PlayerParamSimpleAbilityModel : BaseAbilityModel
    {
        private readonly IActorParamContainer _actorParamContainer;
        private ReactiveParameter _reactiveParameter;

        public PlayerParamSimpleAbilityModel(IActorParamContainer actorParamContainer,AbilitiesModel abilitiesModel, AbilityData data) 
            : base(abilitiesModel, data)
        {
            _actorParamContainer = actorParamContainer;
        }

        public override void Start()
        {
            if (_actorParamContainer.Parameters.TryGetValue(Data.paramType, out _reactiveParameter) == false)
            {
                Debug.LogError($"no parameter {Data.paramType} exist to apply ability {Data.title}");
                Finish();
                return;
            }
            
            TryDropSameParameterAbilities();

            _reactiveParameter.SetValue(Data.value);
            base.Start();
        }

        private void TryDropSameParameterAbilities()
        {
            foreach (var ability in AbilitiesModel.Abilities)
            {
                if (ability.Data.paramType == Data.paramType)
                {
                    ability.Finish();
                }
            }
        }

        public override void Finish()
        {
            base.Finish();
            _reactiveParameter?.ResetToDefaultValue();
        }
    }
}