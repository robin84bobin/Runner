using Data.Catalog;
using Parameters;
using UnityEngine;

namespace Model.Abilities
{
    /// <summary>
    /// Ability unique for concrete actor parameter -
    /// on start drops existed abilities for same parameter,
    /// changes concrete actor parameter to new value
    /// and reset this parameter to default value on finished/dropped.
    /// </summary>
    public class ActorParameterUniqueAbility : BaseAbilityModel
    {
        private readonly IActorParamContainer _actorParamContainer;
        private ReactiveParameter _reactiveParameter;

        public ActorParameterUniqueAbility(IActorParamContainer actorParamContainer, AbilityData data) 
            : base(data)
        {
            _actorParamContainer = actorParamContainer;
        }

        public override void StartAbility()
        {
            if (_actorParamContainer.Parameters.TryGetValue(Data.paramType, out _reactiveParameter) == false)
            {
                Debug.LogError($"no parameter {Data.paramType} exist to apply ability {Data.title}");
                FinishAbility();
                return;
            }

            switch (Data.paramAction)
            {
                case AbilityParamAction.Add:
                    _reactiveParameter.AddValue(Data.value);
                    break;
                default:
                    _reactiveParameter.SetValue(Data.value);
                    break;
            }
            
            base.StartAbility();
        }

        

        public override void FinishAbility()
        {
            base.FinishAbility();
            _reactiveParameter?.ResetToDefaultValue();
        }
    }
}