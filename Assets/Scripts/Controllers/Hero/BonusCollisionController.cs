using Gameplay.Bonuses;
using UnityEngine;

namespace Gameplay.Hero
{
    public class BonusCollisionController : MonoBehaviour
    {
        private IBonusApplier _bonusApplier;

        public void Setup(IBonusApplier bonusApplier)
        {
            _bonusApplier = bonusApplier;
        }
        
        void OnTriggerEnter(Collider other)
        {
            ProcessCollision(other.gameObject);
        }

        void ProcessCollision(GameObject otherGo)
        {
            var bonusControllers = otherGo.GetComponents<BonusController>();
            foreach (var bonusController in bonusControllers)
            {
                _bonusApplier.ApplyBonus(bonusController.Data);
                bonusController.OnApplied();
            }
        }
    }
}