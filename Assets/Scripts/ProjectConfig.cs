using UnityEngine;

[CreateAssetMenu(menuName = "Create ProjectConfig", fileName = "ProjectConfig", order = 0)]
public class ProjectConfig :  ScriptableObject
{
    [Header("DataConfig:")]
    public string CatalogRoot = "Root";
        
    public string CatalogPath = "CatalogData";
    public string GameplayConfigKey = "GameplayConfig";
        
    public string UserRepositoryPath => Application.persistentDataPath + "/user_{0}.json";

    public GameplayConfig GameplayConfig;
}