namespace Installers.Project
{
    public class ProjectMobileInstaller : ProjectCommonInstaller
    {
#if UNITY_ANDROID
        public override void InstallBindings()
        {
            base.InstallBindings();
            //TODO some stuff for platform
        }
#endif
    }
}