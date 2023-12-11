using Data.Catalog;
using Gameplay.Hero;
using Parameters;
using UnityEngine;

namespace Gameplay
{
    public class PlayerParamSimpleAbilityModel : BaseAbilityModel
    {
        private Parameter _parameter;

        public PlayerParamSimpleAbilityModel(HeroModel heroModel, AbilityData data) : base(heroModel, data)
        {
        }

        public override void Start()
        {
            base.Start();
            if (_heroModel.Parameters.TryGetValue(_data.paramType, out _parameter) == false)
            {
                Debug.LogError($"no parameter {_data.paramType} exist to apply ability {_data.title}");
                Finish();
                return;
            }

            _parameter.SetValue(_data.value);
        }

        public override void Finish()
        {
            base.Finish();
            _parameter?.ResetToDefaultValue();
        }
    }
}