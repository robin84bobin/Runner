using Common;
using Data.Catalog;

namespace Services.GamePlay
{
    public class GameCurrentLevelService
    {
        private readonly CatalogDataRepository _catalogDataRepository;
        private readonly IResourcesService _resourcesService;
        private string _levelId;
        
        public string GetHeroPrefabName() => $"Hero{_levelId}";
        public string GetPartPrefabName(int partId) => $"Part{_levelId} {partId}";
        public LevelData LevelData => _catalogDataRepository.Levels.Get(_levelId);

        public GameCurrentLevelService(CatalogDataRepository catalogDataRepository, IResourcesService resourcesService)
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