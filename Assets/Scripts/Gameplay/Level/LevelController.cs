using Cysharp.Threading.Tasks;
using Data.Catalog;
using Services;
using Services.GamePlay;
using UnityEngine;
using Zenject;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Transform heroSpawnPoint;
    [SerializeField] private Transform partsContainer;
    public CharacterController HeroController { get; private set; }

    private LevelModel _model;
    private IResourcesService _resourcesService;
    private GameLevelService _levelService;
    private CatalogDataRepository _catalogDataRepository;
    private LevelPartsController _levelPartsController;

    [Inject]
    public void Construct(
        IGameModel gameModel, 
        GameLevelService levelService, 
        IResourcesService resourcesService,
        CatalogDataRepository catalogDataRepository,
        LevelPartsController levelPartsController
        )
    {
        _levelPartsController = levelPartsController;
        _catalogDataRepository = catalogDataRepository;
        _resourcesService = resourcesService;
        _levelService = levelService;
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
        PartSpawnInfo[] partSpawnInfos = _levelService.CurrentLevelData.parts;
        await _levelPartsController.CreateParts(partSpawnInfos, partsContainer);
    }

    private async UniTask SpawnHero()
    {
        var go = await _resourcesService.Instantiate(
            _levelService.GetHeroPrefabName(), 
            heroSpawnPoint.position, 
            transform.rotation, 
            null
        );

        HeroController = go.GetComponent<CharacterController>();
    }
}