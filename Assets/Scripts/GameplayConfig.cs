using UnityEngine;

[CreateAssetMenu(menuName = "Create GameplayConfig", fileName = "GameplayConfig", order = 0)]
public class GameplayConfig : ScriptableObject
{
    [Header("GameConfig:")]
    public float DefaultSpeed = 0.01f;
    public float MaxSpeed = 0.1f;
    public float MaxHeight = 3f;

    public int VisibleLevelPartCount = 3;
    public int MaxBonusCountPerPart = 1;
    public int RunTrailCount = 3;

    [Header("InputConfig:")]
    public float SwipeTimeThreshold = 1f;
    public float SwipeDistanceThreshold = 0.8f;
    public float MoveTime = 0.2f;

    [Header("For Test")] 
    public bool horizontalMode = false;
}