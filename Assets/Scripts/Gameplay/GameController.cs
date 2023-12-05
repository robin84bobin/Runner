using Cysharp.Threading.Tasks;
using Services;
using Services.GamePlay;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [SerializeField] private LevelController _levelController;
    

    private IGameModel _gameModel;
    private GameLevelService _levelService;
    private IResourcesService _resourcesService;

    private CharacterController _heroController;

    [Inject]
    public void Construct(IGameModel gameModel, GameLevelService levelService, IResourcesService resourcesService)
    {
        _resourcesService = resourcesService;
        _levelService = levelService;
        _gameModel = gameModel;
    }

    async void Start()
    {
        //TODO build level
        await _levelController.BuildLevel();
        //spawn hero, parts, bonusus
    }

   
}