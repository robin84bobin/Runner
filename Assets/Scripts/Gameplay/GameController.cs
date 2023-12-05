using Services;
using Services.GameplayInput;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [SerializeField] private HeroController _heroController;
    [SerializeField] private LevelController _levelController;
    
    private IResourcesService _resourcesService;
    private IGameInputService _inputService;
    private IGameModel _gameModel;

    [Inject]
    public void Construct(IResourcesService resourcesService, IGameInputService inputService, IGameModel gameModel)
    {
        _gameModel = gameModel;
        _inputService = inputService;
        _resourcesService = resourcesService;

        _heroController.Setup(_gameModel.HeroModel);
        _levelController.Setup(_gameModel.LevelModel);
    }
}