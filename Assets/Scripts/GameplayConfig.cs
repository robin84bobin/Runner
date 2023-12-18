using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Settings for gameplay
/// </summary>
[CreateAssetMenu(menuName = "Create GameplayConfig", fileName = "GameplayConfig", order = 0)]
public class GameplayConfig : ScriptableObject
{
    [Header("Hero:")]
    public float DefaultSpeed = 0.01f;
    public float MaxSpeed = 0.1f;
    public float MaxHeight = 3f;
    [Header("Level:")]
    public int VisibleLevelPartCount = 3;
    public int MaxBonusCountPerPart = 1;
    public int RunTrailCount = 3;
    [Header("InputConfig:")]
    public float SwipeTimeThreshold = 1f;
    public float SwipeDistanceThreshold = 0.8f;
    public float StrafeTime = 0.3f;
    public float MoveVerticalTime = 0.8f;

    [Header("For Test")] 
    public bool horizontalMode = false;
}