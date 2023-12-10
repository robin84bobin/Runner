using System.Collections;
using Common;
using Cysharp.Threading.Tasks;
using Data.Catalog;
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
        
        private Transform _hero;
        
        private IGameInputService _inputService;
        private LevelModel _model;
        private IResourcesService _resourcesService;
        private GameCurrentLevelService _currentLevelService;
        private CatalogDataRepository _catalogDataRepository;
        private LevelPartsController _levelPartsController;
        private bool _levelBuilded = false;
        private bool _isMoving = false;
        private Coroutine _moveHeroCoroutine;

        [Inject]
        public void Construct(IGameModel gameModel,
            GameCurrentLevelService currentLevelService,
            IResourcesService resourcesService,
            CatalogDataRepository catalogDataRepository,
            LevelPartsController levelPartsController,
            IGameInputService inputService)
        {
            _inputService = inputService;
            _levelPartsController = levelPartsController;
            _catalogDataRepository = catalogDataRepository;
            _resourcesService = resourcesService;
            _currentLevelService = currentLevelService;
            _model = gameModel.LevelModel;
        }
        
        private async void Start()
        {
            Initialize();
            await BuildLevel();
        }

        private void Initialize()
        {
            _inputService.OnMoveDirection += ProcessMove;
        }

        private async UniTask BuildLevel()
        {
            UniTask[] tasks =
            {
                SpawnHero(),
                CreateLevelParts(),
            };
            await UniTask.WhenAll(tasks);
            _levelBuilded = true;
        }

        private async UniTask CreateLevelParts()
        {
            PartSpawnInfo[] partSpawnInfos = _currentLevelService.LevelData.parts;
            await _levelPartsController.CreateParts(partSpawnInfos, partsContainer);
        }

        private void ProcessMove(Vector2 inputMoveDirection)
        {
            if (!_levelBuilded || _isMoving)
                return;
            
            var moveDirection = inputMoveDirection.NormalizeToHorizontalDirection();
            _moveHeroCoroutine = StartCoroutine(MoveHeroTo(moveDirection, 1f));            
        }

        //TODO move to HeroController
        private IEnumerator MoveHeroTo(Vector2 moveDirection, float moveTime)
        {
            _isMoving = true;

            float startTime = Time.time;
            Vector3 startPosition = _hero.position;
            Vector3 destination = startPosition + new Vector3(moveDirection.x, moveDirection.y);

            while (destination != _hero.position)
            {
                float value = (Time.time - startTime) / moveTime;
                _hero.position = Vector3.Lerp(startPosition, destination, value);
                yield return null;
            }

            _isMoving = false;
        }

        private async UniTask SpawnHero()
        {
            var go = await _resourcesService.Instantiate(
                _currentLevelService.GetHeroPrefabName(), 
                heroSpawnPoint.position, 
                transform.rotation, 
                null
            );

            _hero = go.transform;
        }

        private void OnDestroy()
        {
            if (_moveHeroCoroutine != null)
            {
                StopCoroutine(_moveHeroCoroutine);
                _moveHeroCoroutine = null;
            }

            _inputService.OnMoveDirection -= ProcessMove;
            _levelBuilded = false;
            _isMoving = false;
        }
    }
}