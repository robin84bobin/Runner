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
            BindPlatformInput();
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


#if UNITY_ANDROID || UNITY_IOS
        private void BindPlatformInput()
        {
            Container.BindInterfacesTo<MobileGameInputService>().AsSingle();
        }
#elif UNITY_EDITOR || UNITY_STANDALONE
        private void BindPlatformInput()
        {
            Container.Bind<IGameInputService>().To<StandaloneGameInputService>().AsSingle();
        }
#endif

        private void BindModels()
        {
            Container.BindInterfacesAndSelfTo<AbilitiesModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ActorModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameModel>().AsSingle();
        }
    }
}