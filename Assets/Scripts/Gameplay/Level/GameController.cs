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
        private float _moveValue;
        private ProjectConfig _config;
        private float _leftXBound;
        private float _rightXBound;

        [Inject]
        public void Construct(IGameModel gameModel,
            ProjectConfig config,
            GameCurrentLevelService currentLevelService,
            IResourcesService resourcesService,
            CatalogDataRepository catalogDataRepository,
            LevelPartsController levelPartsController,
            IGameInputService inputService)
        {
            _config = config;
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
            //TODO move to heroMoveController
            var boxCollider = partsContainer.GetComponent<BoxCollider>();
            _moveValue = boxCollider.size.x/ _config.RunTrailCount;
            _leftXBound = partsContainer.transform.TransformPoint(boxCollider.center - boxCollider.size * 0.5f).x;
            _rightXBound = partsContainer.transform.TransformPoint(boxCollider.center + boxCollider.size*0.5f).x;
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

        //TODO move to heroMoveController
        private void ProcessMove(Vector2 inputMoveDirection)
        {
            if (!_levelBuilded || _isMoving)
                return;

            var moveDirection = inputMoveDirection.NormalizeToHorizontalDirection(_moveValue);
            _moveHeroCoroutine = StartCoroutine(MoveHeroTo(moveDirection, _config.MoveTime));            
        }

        //TODO move to heroMoveController
        private IEnumerator MoveHeroTo(Vector2 moveDirection, float moveTime)
        {
            float startTime = Time.time;
            Vector3 startPosition = _hero.position;
            Vector3 destination = startPosition + new Vector3(moveDirection.x, moveDirection.y);


            if (destination.x > _rightXBound || destination.x < _leftXBound)
                yield break;
            
            _isMoving = true;
            while (destination != _hero.position)
            {
                float value = (Time.time - startTime) / moveTime;
                _hero.position = Vector3.Slerp(startPosition, destination, value);
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