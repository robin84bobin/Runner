using Services.GamePlay;
using Services.GameplayInput;
using UnityEngine;
using Zenject;

public class GameModel : IGameModel, ITickable
{
    private readonly IGameInputService _inputService;
    public LevelModel LevelModel { get; }
    public HeroModel HeroModel { get; }

    public GameModel(GameplayLevelService gameplayLevelService, IGameInputService inputService)
    {
        _inputService = inputService;
        
        var levelData = gameplayLevelService.CurrentLevelData;
        this.LevelModel = new LevelModel(levelData);


        var heroPrefabName = gameplayLevelService.HeroPrefabName;
        this.HeroModel = new HeroModel(heroPrefabName);
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