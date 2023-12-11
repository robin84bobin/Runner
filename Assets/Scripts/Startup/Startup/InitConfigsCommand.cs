using Cysharp.Threading.Tasks;
using Services;

namespace Commands.Startup
{
    public class InitConfigsCommand : Command
    {
        private readonly ProjectConfig _projectConfig;
        private readonly IResourcesService _resourcesService;

        public InitConfigsCommand(ProjectConfig projectConfig, IResourcesService resourcesService)
        {
            _projectConfig = projectConfig;
            _resourcesService = resourcesService;
        }
        
        public override async UniTask Execute()
        {
            var gameplayConfig = await _resourcesService.LoadAsset<GameplayConfig>(_projectConfig.GameplayConfigKey);
            _projectConfig.GameplayConfig = gameplayConfig;
            Complete();
        }
    }
}