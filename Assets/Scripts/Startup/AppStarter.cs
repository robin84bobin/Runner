using Core.Common.Commands;
using Core.Data;
using Data.Catalog;
using Services.Resources;
using Startup.Startup;
using UnityEngine;
using Zenject;

namespace Startup
{
    public class AppStarter : MonoBehaviour
    {
        private IResourcesService _resourcesService;
        private CatalogDataRepository _catalogRepository;
        private ProjectConfig _projectConfig;
        private CommandSerialSequence _commandSequence;

        [Inject]
        public void Construct(
            IResourcesService resourcesService, 
            CatalogDataRepository catalogRepository, 
            ProjectConfig projectConfig)
        {
            _resourcesService = resourcesService;
            _catalogRepository = catalogRepository;
            _projectConfig = projectConfig;
        }

        async void Start()
        {
            _commandSequence = new CommandSerialSequence(
                new InitConfigsCommand(_projectConfig, _resourcesService),
                new InitDataRepositoryCommand(_catalogRepository)
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
