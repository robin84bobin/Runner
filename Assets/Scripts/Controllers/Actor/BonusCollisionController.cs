using Controllers.Bonuses;
using Model;
using UnityEngine;

namespace Controllers.Actor
{
    /// <summary>
    /// Applies bonus if hits it by trigger collision 
    /// </summary>
    public class BonusCollisionController : MonoBehaviour
    {
        private AbilityService _abilityService;
        private ActorModel _actorModel;

        public void Setup(AbilityService abilityService, ActorModel actorModel)
        {
            _actorModel = actorModel;
            _abilityService = abilityService;
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
                _abilityService.ApplyAbilities(bonusController.BonusId, _actorModel);
                bonusController.OnApplied();
            }
        }
    }
}