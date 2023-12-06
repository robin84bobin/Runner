using Data.Catalog;
using Data.Proxy;
using Data.User;
using Services;
using Services.GamePlay;
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
            Container.Bind<GameCurrentLevelService>().AsSingle();
        }

        private void BindResourcesService()
        {
            Container.Bind<IResourcesService>().To<AddressablesResourcesService>()
                .AsSingle().NonLazy();
            Container.Bind<IResourcesService>().To<UnityResourcesService>().AsSingle()
                .WhenInjectedInto<UserDataRepository>().NonLazy();
        }

        private void BindDataProxies()
        {
            Container.Bind<IDataProxyService>().To<JsonDataProxyService>().WithArguments(_projectConfig.CatalogPath, _projectConfig.CatalogRoot)
                .WhenInjectedInto<CatalogDataRepository>();
            Container.Bind<IDataProxyService>().To<JsonDataProxyService>().WithArguments(_projectConfig.UserRepositoryPath, _projectConfig.CatalogRoot)
                .WhenInjectedInto<UserDataRepository>();
        }

        private void BindDataRepositories()
        {
            Container.Bind<CatalogDataRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<UserDataRepository>().AsSingle();
        }
    }
}