using Services.GamePlay;
using Services.GameplayInput;
using UnityEngine;
using Zenject;

public class GameModel : IGameModel, ITickable
{
    private readonly IGameInputService _inputService;
    public LevelModel LevelModel { get; }
    public HeroModel HeroModel { get; }

    public GameModel(GameLevelService gameLevelService, IGameInputService inputService)
    {
        _inputService = inputService;
        
        var levelData = gameLevelService.CurrentLevelData;
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