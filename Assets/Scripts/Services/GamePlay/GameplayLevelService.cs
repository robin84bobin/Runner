using Common;
using Data.Catalog;
using UnityEngine.SceneManagement;

namespace Services.GamePlay
{
    public class GameplayLevelService
    {
        private readonly CatalogDataRepository _catalogDataRepository;
        private readonly IResourcesService _resourcesService;
        private string _levelId;
        
        public string HeroPrefabName => $"Hero{_levelId}";
        public LevelData CurrentLevelData => _catalogDataRepository.Levels.Get(_levelId);


        public GameplayLevelService(CatalogDataRepository catalogDataRepository, IResourcesService resourcesService)
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