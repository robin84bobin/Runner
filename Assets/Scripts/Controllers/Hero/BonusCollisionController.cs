using Controllers.Bonuses;
using Model;
using UnityEngine;

namespace Controllers.Hero
{
    /// <summary>
    /// Applies bonus if hits it by trigger collision 
    /// </summary>
    public class BonusCollisionController : MonoBehaviour
    {
        private AbilitiesModel _abilitiesModel;

        public void Setup(AbilitiesModel abilitiesModel)
        {
            _abilitiesModel = abilitiesModel;
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
                _abilitiesModel.ApplyAbilities(bonusController.BonusId);
                bonusController.OnApplied();
            }
        }
    }
}