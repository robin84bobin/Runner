using Common.Commands;
using Cysharp.Threading.Tasks;
using Services.Resources;

namespace Startup.Startup
{
    /// <summary>
    /// load gameplay config from resources and set it into project config
    /// *to make possible update gameplay config via bundles* 
    /// </summary>
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