using UnityEngine;

namespace Installers.Project
{
    [CreateAssetMenu(menuName = "Create ProjectConfig", fileName = "ProjectConfig", order = 0)]
    internal class ProjectConfig :  ScriptableObject
    {
        [Header("DataConfig:")]
        public string CatalogRoot = "Root";
        
        public string CatalogPath => "CatalogData";
        
        public string UserRepositoryPath => Application.persistentDataPath + "/user_{0}.json";
    }
}