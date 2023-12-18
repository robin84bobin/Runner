using Controllers.Level.Parts;
using Controllers.Level.Parts.PartsMoving;
using Model;
using Services.GamePlay.GameplayInput;
using UnityEngine;
using Zenject;

namespace DI.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        private ProjectConfig _projectConfig;

        public override void InstallBindings()
        {
            BindConfigs();
            BindPlatformDependencies();
            BindModels();
            BindLevelParts();
        }

        private void BindConfigs()
        {
            _projectConfig = Container.Resolve<ProjectConfig>();
            Container.Bind<GameplayConfig>().FromInstance(_projectConfig.GameplayConfig);
        }

        private void BindLevelParts()
        {
            Container.BindInterfacesAndSelfTo<LevelController>().AsSingle();
            if (_projectConfig.GameplayConfig.horizontalMode)
            {
                Container.Bind<IMoveLevelPartsStrategy>().To<HorizontalMoveLevelPartsStrategy>().AsSingle();
            }
            else
            {
                Container.Bind<IMoveLevelPartsStrategy>().To<VerticalMoveLevelPartsStrategy>().AsSingle();
            }
        }


#if UNITY_EDITOR|| UNITY_STANDALONE
        private void BindPlatformDependencies()
        {
            Container.BindInterfacesTo<StandaloneGameInputService>().AsSingle();
        }
#elif UNITY_ANDROID || UNITY_IOS && !UNITY_EDITOR
        private void BindPlatformDependencies()
        {
            Container.BindInterfacesTo<MobileGameInputService>().AsSingle();
        }
#endif

        private void BindModels()
        {
            Container.BindInterfacesAndSelfTo<AbilityService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ActorModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameModel>().AsSingle();
        }
    }
}