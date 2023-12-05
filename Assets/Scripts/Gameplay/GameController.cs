using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [SerializeField] private HeroController _heroController;
    [SerializeField] private LevelController _levelController;
    
    private IGameModel _gameModel;

    [Inject]
    public void Construct(IGameModel gameModel)
    {
        _gameModel = gameModel;
        
        _heroController.Setup(_gameModel.HeroModel);
        _levelController.Setup(_gameModel.LevelModel);
    }

    private void Start()
    {
        //TODO build level
        //spawn hero, parts, bonusus
    }
    
}