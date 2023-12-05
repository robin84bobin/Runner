using Services.GameplayInput;
using UnityEngine;
using Zenject;

namespace Installers.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private ObjectPool _objectPool;
        
        public override void InstallBindings()
        {
            BindPlatformInput();
            BindModel();
            Container.BindInterfacesAndSelfTo<ScrollController>().AsSingle();
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