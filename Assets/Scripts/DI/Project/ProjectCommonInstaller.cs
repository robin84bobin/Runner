using Data.Catalog;
using Data.Proxy;
using Services.GamePlay;
using Services.Resources;
using UnityEngine;
using Zenject;

namespace DI.Project
{
    public class ProjectCommonInstaller : MonoInstaller 
    {
        [SerializeField] private ProjectConfig _projectConfig;

        public override void InstallBindings()
        {
            BindConfigs();
            BindResourcesService();
            BindDataProxies();
            BindDataRepositories();
            BindGameplayServices();
        }

        private void BindConfigs()
        {
            Container.Bind<ProjectConfig>().FromInstance(_projectConfig).AsSingle();
        }

        private void BindGameplayServices()
        {
            Container.Bind<GameLevelService>().AsSingle();
        }

        private void BindResourcesService()
        {
            Container.Bind<IResourcesService>().To<AddressablesResourcesService>()
                .AsSingle().NonLazy();
        }

        private void BindDataProxies()
        {
            Container.Bind<IDataProxyService>().To<JsonDataProxyService>().WithArguments(_projectConfig.CatalogPath, _projectConfig.CatalogRoot)
                .WhenInjectedInto<CatalogDataRepository>();
        }

        private void BindDataRepositories()
        {
            Container.Bind<CatalogDataRepository>().AsSingle();
        }
    }
}