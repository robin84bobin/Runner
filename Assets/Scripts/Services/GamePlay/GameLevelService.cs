using Common;
using Data.Catalog;

namespace Services.GamePlay
{
    public class GameLevelService
    {
        private readonly CatalogDataRepository _catalogDataRepository;
        private readonly IResourcesService _resourcesService;
        private string _levelId;
        
        public string GetHeroPrefabName() => $"Hero{_levelId}";
        public string GetPartPrefabName(int partId) => $"Part{_levelId} {partId}";
        public LevelData CurrentLevelData => _catalogDataRepository.Levels.Get(_levelId);

        public GameLevelService(CatalogDataRepository catalogDataRepository, IResourcesService resourcesService)
        {
            _catalogDataRepository = catalogDataRepository;
            _resourcesService = resourcesService;
        }

        public void StartLevel(string levelId)
        {
            _levelId = levelId;
            _resourcesService.LoadScene(AppConstants.Scenes.Game);
        }

    }
}