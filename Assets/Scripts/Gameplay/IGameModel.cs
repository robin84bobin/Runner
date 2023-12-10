using Data.Catalog;
using Gameplay.Hero;

namespace Gameplay
{
    public interface IGameModel
    {
        HeroModel HeroModel { get; }
        void ApplyBonus(BonusData data);
    }
}