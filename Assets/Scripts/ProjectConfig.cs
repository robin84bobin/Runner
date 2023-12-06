using UnityEngine;

[CreateAssetMenu(menuName = "Create ProjectConfig", fileName = "ProjectConfig", order = 0)]
public class ProjectConfig :  ScriptableObject
{
    [Header("DataConfig:")]
    public string CatalogRoot = "Root";
        
    public string CatalogPath = "CatalogData";
        
    public string UserRepositoryPath => Application.persistentDataPath + "/user_{0}.json";
        
    [Header("GameConfig:")]
    public float DefaultSpeed = -0.01f;
    public int VisibleLevelPartCount = 3;
    public int MaxBonusCountPerPart = 1;
}