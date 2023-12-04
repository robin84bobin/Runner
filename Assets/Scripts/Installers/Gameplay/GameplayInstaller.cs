using System;
using Services.GameplayInput;
using Zenject;

namespace Installers.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindPlatformInput();
        }

#if UNITY_ANDROID || UNITY_IOS
        
        private void BindPlatformInput()
        {
            Container.Bind<IGameInputService>().To<MobileGameInputService>().AsCached();
            Container.Bind<IDisposable>().To<MobileGameInputService>().AsCached();
        }
        
#elif UNITY_EDITOR || UNITY_STANDALONE

        private void BindPlatformInput()
        {
            Container.Bind<IGameInputService>().To<StandaloneGameInputService>().AsSingle();
        }
        
#endif

    }
}