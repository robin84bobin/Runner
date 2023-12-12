using UnityEngine;

namespace Controllers.Bonuses
{
    /// <summary>
    /// Component to store bonusId of bonus for processing by collision of gameobject
    /// </summary>
    public class BonusController : MonoBehaviour
    {
        public string BonusId { get; private set; }

        public void Setup(string bonusId)
        {
            BonusId = bonusId;
        }

        public void OnApplied()
        {
            Destroy(gameObject);
        }
    }
}