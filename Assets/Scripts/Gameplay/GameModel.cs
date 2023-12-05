using Services.GamePlay;

public class GameModel : IGameModel
{
    public LevelModel LevelModel { get; }
    public HeroModel HeroModel { get; }

    public GameModel(GameplayLevelService gameplayLevelService, int levelId)
    {
        var levelData = gameplayLevelService.CurrentLevelData;
        this.LevelModel = new LevelModel(levelData);


        var heroPrefabName = gameplayLevelService.HeroPrefabName;
        this.HeroModel = new HeroModel(heroPrefabName);
    }
}