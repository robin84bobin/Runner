namespace Installers.Project
{
    public class ProjectStandaloneInstaller : ProjectCommonInstaller
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        public override void InstallBindings()
        {
            base.InstallBindings();
            //TODO some stuff for platform
        }
#endif
    }
}