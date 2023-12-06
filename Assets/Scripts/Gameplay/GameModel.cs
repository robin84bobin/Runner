using Gameplay.Hero;
using Gameplay.Level;
using Services.GamePlay;
using Services.GamePlay.GameplayInput;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class GameModel : IGameModel, ITickable
    {
        private readonly IGameInputService _inputService;
        public LevelModel LevelModel { get; }
        public HeroModel HeroModel { get; }

        public GameModel(GameCurrentLevelService gameCurrentLevelService, IGameInputService inputService)
        {
            _inputService = inputService;
        
            var levelData = gameCurrentLevelService.LevelData;
            LevelModel = new LevelModel(levelData);

            HeroModel = new HeroModel();
        }

        public void Tick()
        {
            var inputMoveDirection = _inputService.GetInputMoveDirection();
            if (inputMoveDirection != Vector2.zero)
            {
                HeroModel.ProcessInputMove(inputMoveDirection);
            }
        }
    }
}