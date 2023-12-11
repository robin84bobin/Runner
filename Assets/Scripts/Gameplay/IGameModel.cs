using Gameplay.Hero;

namespace Gameplay
{
    public interface IGameModel: IBonusApplier
    {
        HeroModel HeroModel { get; }
    }
}