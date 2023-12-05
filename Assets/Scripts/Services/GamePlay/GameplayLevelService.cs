using Data.Catalog;

namespace Services.GamePlay
{
    public class GameplayLevelService
    {
        private readonly CatalogDataRepository _catalogDataRepository;
        private string _levelId;
        
        public string HeroPrefabName => $"Hero{_levelId}";
        public LevelData CurrentLevelData => _catalogDataRepository.Levels.Get(_levelId);


        public GameplayLevelService(CatalogDataRepository catalogDataRepository)
        {
            _catalogDataRepository = catalogDataRepository;
        }

        public void SetupLevel(int level)
        {
            _levelId = level.ToString();
        }
    }
}