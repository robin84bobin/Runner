using Services.GameplayInput;
using Zenject;

namespace Installers.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindPlatformInput();
            BindModel();
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