using Gameplay.Level;
using Services;
using Services.GamePlay;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private LevelController _levelController;

        private IGameModel _gameModel;
        private GameCurrentLevelService _currentLevelService;
        private IResourcesService _resourcesService;

        private CharacterController _heroController;

        [Inject]
        public void Construct(IGameModel gameModel, GameCurrentLevelService currentLevelService, IResourcesService resourcesService)
        {
            _resourcesService = resourcesService;
            _currentLevelService = currentLevelService;
            _gameModel = gameModel;
        }

        async void Start()
        {
            await _levelController.BuildLevel();
        }
   
    }
}