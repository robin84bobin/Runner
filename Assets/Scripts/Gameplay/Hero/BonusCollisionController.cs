using Gameplay.Bonuses;
using UnityEngine;

namespace Gameplay.Hero
{
    public class BonusCollisionController : MonoBehaviour
    {
        private IGameModel _gameModel;

        public void Setup(IGameModel gameModel)
        {
            _gameModel = gameModel;
        }
        
        void OnTriggerEnter(Collider other)
        {
            ProcessCollision(other.gameObject);
        }

        void ProcessCollision(GameObject otherGo)
        {
            var bonusControllers = otherGo.GetComponents<BonusController>();
            for (int i = 0; i < bonusControllers.Length; ++i)
            {
                //TODO use Signals
               _gameModel.ApplyBonus(bonusControllers[i].Data);
            }
        }


    }
}