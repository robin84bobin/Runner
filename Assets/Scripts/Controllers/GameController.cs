using Controllers.Hero;
using Controllers.Level.Parts;
using Cysharp.Threading.Tasks;
using Data.Catalog;
using Model;
using Services.GamePlay;
using Services.GamePlay.GameplayInput;
using Services.Resources;
using UnityEngine;
using Zenject;

namespace Controllers
{
    /// <summary>
    /// Builds game level and hero
    /// </summary>
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Transform heroSpawnPoint;
        [SerializeField] private Transform partsContainer;

        private IResourcesService _resourcesService;
        private GameLevelService _levelService;
        private LevelController _levelController;
        private GameplayConfig _gameplayConfig;
        private IGameInputService _inputService;
        private GameModel _gameModel;
        
        private ActorMoveController _actorMoveController;
        private BonusCollisionController _bonusCollisionController;

        [Inject]
        public void Construct(
            GameModel gameModel,
            GameLevelService levelService,
            IResourcesService resourcesService,
            LevelController levelController,
            GameplayConfig gameplayConfig,
            IGameInputService inputService)
        {
            _gameModel = gameModel;
            _gameplayConfig = gameplayConfig;
            _inputService = inputService;
            _levelController = levelController;
            _resourcesService = resourcesService;
            _levelService = levelService;
        }

        private async void Start()
        {
            //TODO show overlay UI to hide building level 
            //TODO disable IGameplayInputService
            await BuildLevel();
            //TODO show start button in overlay UI 
            //TODO on start button click - hide overlay UI and enable IGameplayInputService
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
            PartSpawnInfo[] partSpawnInfos = _levelService.LevelData.parts;
            await _levelController.CreateParts(partSpawnInfos, partsContainer);
        }

        private async UniTask SpawnHero()
        {
            var go = await _resourcesService.Instantiate(
                _levelService.GetHeroPrefabName(),
                heroSpawnPoint.position,
                transform.rotation,
                null
            );

            _bonusCollisionController = go.GetComponent<BonusCollisionController>();
            _bonusCollisionController.Setup(_gameModel.AbilitiesModel);
            
            _actorMoveController = go.GetComponent<ActorMoveController>();
            _actorMoveController.Setup(_gameplayConfig, _inputService, partsContainer, _gameModel.ActorModel);
        }
    }
}