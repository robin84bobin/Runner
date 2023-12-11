using Data.Catalog;
using Gameplay.Hero;

namespace Gameplay
{
    public class FlyAbilityModel : BaseAbilityModel
    {
        public FlyAbilityModel(HeroModel heroModel, AbilityData data) : base(heroModel, data)
        {
        }

        public override void Start()
        {
            base.Start();
            _heroModel.Height.SetValue(_data.value);
        }

        public override void Finish()
        {
            base.Finish();
            _heroModel.Height.ResetToDefaultValue();
        }
    }
}