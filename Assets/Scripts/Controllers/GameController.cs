using Cysharp.Threading.Tasks;
using Data.Catalog;
using Gameplay.Hero;
using Gameplay.Level.Parts;
using Services;
using Services.GamePlay;
using Services.GamePlay.GameplayInput;
using UnityEngine;
using Zenject;

namespace Gameplay.Level
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Transform heroSpawnPoint;
        [SerializeField] private Transform partsContainer;

        private IResourcesService _resourcesService;
        private GameCurrentLevelService _currentLevelService;
        private CatalogDataRepository _catalogDataRepository;
        private LevelController _levelController;
        private GameplayConfig _gameplayConfig;
        private IGameInputService _inputService;
        private IGameModel _gameModel;
        
        private InputMoveController _heroMoveController;
        private BonusCollisionController _bonusCollisionController;
        private HeroHeightController _heroHeightController;

        [Inject]
        public void Construct(
            IGameModel gameModel,
            GameCurrentLevelService currentLevelService,
            IResourcesService resourcesService,
            CatalogDataRepository catalogDataRepository,
            LevelController levelController,
            GameplayConfig gameplayConfig,
            IGameInputService inputService)
        {
            _gameModel = gameModel;
            _gameplayConfig = gameplayConfig;
            _inputService = inputService;
            _levelController = levelController;
            _catalogDataRepository = catalogDataRepository;
            _resourcesService = resourcesService;
            _currentLevelService = currentLevelService;
        }

        private async void Start()
        {
            await BuildLevel();
        }


        private async UniTask BuildLevel()
        {
            UniTask[] tasks =
            {
                SpawnHero(),
                CreateLevelParts(),
            };
            await UniTask.WhenAll(tasks);
        }

        private async UniTask CreateLevelParts()
        {
            PartSpawnInfo[] partSpawnInfos = _currentLevelService.LevelData.parts;
            await _levelController.CreateParts(partSpawnInfos, partsContainer);
        }

        private async UniTask SpawnHero()
        {
            var go = await _resourcesService.Instantiate(
                _currentLevelService.GetHeroPrefabName(),
                heroSpawnPoint.position,
                transform.rotation,
                null
            );

            _bonusCollisionController = go.GetComponent<BonusCollisionController>();
            _bonusCollisionController.Setup(_gameModel);
            
            _heroHeightController = go.GetComponent<HeroHeightController>();
            _heroHeightController.Setup(_gameModel.HeroModel, -heroSpawnPoint.position.z);
            
            _heroMoveController = go.GetComponent<InputMoveController>();
            _heroMoveController.Setup(_gameplayConfig, _inputService, partsContainer);
        }
    }
}