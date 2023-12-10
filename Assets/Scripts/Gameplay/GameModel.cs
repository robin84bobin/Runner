using Gameplay.Hero;
using Gameplay.Level;
using Services.GamePlay;

namespace Gameplay
{
    public class GameModel : IGameModel
    {
        public LevelModel LevelModel { get; }
        public HeroModel HeroModel { get; }

        public GameModel(GameCurrentLevelService gameCurrentLevelService)
        {
            //TODO remove models?
            var levelData = gameCurrentLevelService.LevelData;
            LevelModel = new LevelModel(levelData);
            HeroModel = new HeroModel();
            //
        }
    }
}