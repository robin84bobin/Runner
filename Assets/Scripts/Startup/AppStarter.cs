using Commands;
using Commands.Startup;
using Common;
using Data.Catalog;
using Data.User;
using Services;
using UnityEngine;
using Zenject;

namespace Bootstrap
{
    public class AppStarter : MonoBehaviour
    {
        [Inject] private IResourcesService _resourcesService;
        [Inject] private CatalogDataRepository _catalogRepository;
        [Inject] private UserDataRepository _userRepository;
        [Inject] private ProjectConfig _projectConfig;

        private CommandSerialSequence _commandSequence;
    
        async void Start()
        {
            _commandSequence = new CommandSerialSequence(
                new InitConfigsCommand(_projectConfig, _resourcesService),
                new InitDataRepositoryCommand(_catalogRepository),
                new InitDataRepositoryCommand(_userRepository)
            );
            _commandSequence.OnComplete += OnInitComplete;
            _commandSequence.OnProgress += OnInitProgress;
            await _commandSequence.Execute();
        }

        private void OnInitProgress(float percent)
        {
            Debug.Log($"{this} : {percent * 100} %");
        }

        private void OnInitComplete()
        {
            _commandSequence.OnProgress -= OnInitProgress;
            _commandSequence.OnComplete -= OnInitComplete;
            _resourcesService.LoadScene(AppConstants.Scenes.Start);
        }
    
    }
}
