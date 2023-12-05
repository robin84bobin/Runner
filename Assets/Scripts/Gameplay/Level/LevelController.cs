using UnityEngine;

public class LevelController : MonoBehaviour
{
    private LevelModel _model;

    public void Setup(LevelModel gameModelLevelModel)
    {
        _model = gameModelLevelModel;
    }
}