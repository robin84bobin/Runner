using Gameplay;
using Gameplay.Level.Parts;
using Gameplay.Level.Parts.PartsMoving;
using Services.GamePlay.GameplayInput;
using UnityEngine;
using Zenject;

namespace DI.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private ObjectPool _objectPool;
        
        public override void InstallBindings()
        {
            BindPlatformInput();
            BindModel();
            BindObjectPool();
            BindLevelParts();
        }

        private void BindLevelParts()
        {
            Container.BindInterfacesAndSelfTo<LevelController>().AsSingle();
            // Container.Bind<IMoveLevelPartsStrategy>().To<HorizontalMoveLevelPartsStrategy>().AsSingle();
            Container.Bind<IMoveLevelPartsStrategy>().To<VerticalMoveLevelPartsStrategy>().AsSingle();
        }

        private void BindObjectPool()
        {
            Container.Bind<ObjectPool>().FromInstance(_objectPool).AsSingle();
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

        private void BindModel()
        {
            Container.BindInterfacesTo<GameModel>().AsSingle();
        }
    }
}