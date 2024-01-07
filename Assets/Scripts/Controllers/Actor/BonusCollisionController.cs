using System.Threading.Tasks;
using Controllers.Bonuses;
using Cysharp.Threading.Tasks;
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
        private BonusController _bonusController;
        private IBonusController _iBonusController;

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
            _bonusController = otherGo.GetComponent<BonusController>();
            _iBonusController = _bonusController;
            _abilityService.ApplyAbilities(_bonusController.BonusId, _actorModel);
            
            _bonusController.OnApplied();
        }
    }
}