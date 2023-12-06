using Cysharp.Threading.Tasks;
using Data.Catalog;
using Gameplay.Level.Parts;
using Services;
using Services.GamePlay;
using UnityEngine;
using Zenject;

namespace Gameplay.Level
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private Transform heroSpawnPoint;
        [SerializeField] private Transform partsContainer;
        public CharacterController HeroController { get; private set; }

        private LevelModel _model;
        private IResourcesService _resourcesService;
        private GameCurrentLevelService _currentLevelService;
        private CatalogDataRepository _catalogDataRepository;
        private LevelPartsController _levelPartsController;

        [Inject]
        public void Construct(
            IGameModel gameModel, 
            GameCurrentLevelService currentLevelService, 
            IResourcesService resourcesService,
            CatalogDataRepository catalogDataRepository,
            LevelPartsController levelPartsController
        )
        {
            _levelPartsController = levelPartsController;
            _catalogDataRepository = catalogDataRepository;
            _resourcesService = resourcesService;
            _currentLevelService = currentLevelService;
            _model = gameModel.LevelModel;
        }

        public async UniTask BuildLevel()
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
            await _levelPartsController.CreateParts(partSpawnInfos, partsContainer);
        }

        private async UniTask SpawnHero()
        {
            var go = await _resourcesService.Instantiate(
                _currentLevelService.GetHeroPrefabName(), 
                heroSpawnPoint.position, 
                transform.rotation, 
                null
            );

            HeroController = go.GetComponent<CharacterController>();
        }
    }
}