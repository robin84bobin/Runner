using Core.Data;
using Data.Catalog;
using Services.Resources;

namespace Services.GamePlay
{
    /// <summary>
    /// Starts level.
    /// Encapsulate levelId for current level
    /// to get some data for current level
    /// </summary>
    public class GameLevelService
    {
        private readonly CatalogDataRepository _catalogDataRepository;
        private readonly IResourcesService _resourcesService;
        private string _levelId;
        
        public string GetHeroPrefabName() => $"Hero{_levelId}";
        public string GetPartPrefabName(string partId) => $"Part{_levelId} {partId}";
        public LevelData LevelData => _catalogDataRepository.Levels.Get(_levelId);

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