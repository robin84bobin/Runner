using Gameplay.Hero;
using Gameplay.Level;

namespace Gameplay
{
    public interface IGameModel
    {
        LevelModel LevelModel { get; }
        HeroModel HeroModel { get; }
    }
}