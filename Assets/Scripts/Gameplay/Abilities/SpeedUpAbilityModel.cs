using Data.Catalog;
using Gameplay.Hero;

namespace Gameplay
{
    public class SpeedUpAbilityModel : BaseAbilityModel
    {
        public SpeedUpAbilityModel(HeroModel heroModel, AbilityData data) : base(heroModel, data)
        {
        }

        public override void Start()
        {
            base.Start();
            _heroModel.Speed.SetValue(_data.value);
        }

        public override void Finish()
        {
            base.Finish();
            _heroModel.Speed.ResetToDefaultValue();
        }
    }
}